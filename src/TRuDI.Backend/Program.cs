namespace TRuDI.Backend
{
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.Razor;
    using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Hosting;
    using TRuDI.Backend.MessageHandlers;
    using TRuDI.Backend.Utils;
    using TRuDI.TafAdapter.Interface;
    using TRuDI.TafAdapter.Repository;
    using WebSocketManager;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.CommandLineUtils;

    using Serilog;
    using Serilog.Core;
    using Serilog.Events;

    using TRuDI.Backend.Application;
    using TRuDI.HanAdapter.Repository;

    using Microsoft.AspNetCore.Server.Kestrel.Https;

    public class Program
    {
        private static int backendServerPort;

        public static CommandLineArguments CommandLineArguments { get; } = new CommandLineArguments();

        /// <summary>
        /// The application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);
            var logFileOption = commandLineApplication.Option("-l|--log <logfile>", "Datei in der Programmmeldungen protokolliert werden.", CommandOptionType.SingleValue);
            var logLevelOption = commandLineApplication.Option("--loglevel <loglevel>", "Log Level: verbose, debug, info, warning, error, fatal. Standard ist info", CommandOptionType.SingleValue);
            var testFileOption = commandLineApplication.Option("-t|--test <testconfig>", "Aktiviert den Test-HAN-Adapter mit der angegebenen Konfigurationsdatei.", CommandOptionType.SingleValue);

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.Execute(args);
#if DEBUG
            var logLevelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Verbose };
#else
            var logLevelSwitch = new LoggingLevelSwitch { MinimumLevel = LogEventLevel.Information };
#endif

            if (logLevelOption.HasValue())
            {
                switch (logLevelOption.Value().ToLowerInvariant())
                {
                    case "verbose":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Verbose;
                        break;

                    case "debug":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Debug;
                        break;

                    case "info":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Information;
                        break;

                    case "warning":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Warning;
                        break;

                    case "error":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Error;
                        break;

                    case "fatal":
                        logLevelSwitch.MinimumLevel = LogEventLevel.Fatal;
                        break;
                }
            }

            if (logFileOption.HasValue())
            {
                Console.WriteLine("Using log file: {0}", logFileOption.Value());

                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(logLevelSwitch)
#if DEBUG
                    .WriteTo.Trace()
#endif
                    .WriteTo.ColoredConsole()
                    .WriteTo.File(logFileOption.Value())
                    .CreateLogger();
            }
            else
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(logLevelSwitch)
#if DEBUG
                    .WriteTo.Trace()
#endif
                    .WriteTo.ColoredConsole()
                    .CreateLogger();
            }

            var fileVersionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            Log.Information(
                "Starting TRuDI {0} on {1}",
                fileVersionInfo.FileVersion,
                System.Runtime.InteropServices.RuntimeInformation.OSDescription);

            if (testFileOption.HasValue())
            {
                // Check for a configuration file/directory used by the example/simulation HAN adapter
                var testConfigFile = testFileOption.Value();
                if (!string.IsNullOrWhiteSpace(testConfigFile)
                    && (File.Exists(testConfigFile) || Directory.Exists(testConfigFile)))
                {
                    Log.Information("Using test configuration file: {0}", testConfigFile);

                    CommandLineArguments.TestConfiguration = testConfigFile;
                    HanAdapterRepository.ActivateExampleHanAdapter();
                }
                else
                {
                    Log.Warning("Configuration file not found: {0}", testConfigFile);
                }
            }

            var cultureInfo = new CultureInfo("de-DE");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var builder = WebApplication.CreateBuilder();

            builder.Services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
            });

            builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                foreach (var hanAdapterInfo in HanAdapterRepository.AvailableAdapters)
                {
                    options.FileProviders.Add(new EmbeddedFileProvider(hanAdapterInfo.Assembly, hanAdapterInfo.BaseNamespace));
                }

                foreach (var tafAdapterInfo in TafAdapterRepository.AvailableAdapters)
                {
                    options.FileProviders.Add(new EmbeddedFileProvider(tafAdapterInfo.Assembly, tafAdapterInfo.BaseNamespace));
                }
            });

            builder.Services.AddWebSocketManager();
            builder.Services.AddSingleton<ApplicationState>();
            builder.Services.AddTransient<ITafData, ITafData>((ctx) =>
            {
                var state = ctx.GetService<ApplicationState>();
                return state.CurrentSupplierFile?.TafData?.Data;
            });

            builder.WebHost.ConfigureKestrel(options =>
            {
#if DEBUG
                options.Listen(IPAddress.Loopback, 5000);
                backendServerPort = 5000;
#else
                // Get free TCP port and write it to STDOUT where the Electron frontend can catch it.
                backendServerPort = FindFreeTcpPort();
                options.Listen(IPAddress.Loopback, backendServerPort, listenOptions =>
                    {
                        var httpsOptions =
                            new Microsoft.AspNetCore.Server.Kestrel.Https.HttpsConnectionAdapterOptions()
                            {
                                ServerCertificate =
                                        CertificateGenerator
                                            .GenerateCertificate(
                                                $"CN={DigestUtils.GetRipemd160DigestFromAssembly(typeof(Program).Assembly).ToLowerInvariant()}")
                            };

                        listenOptions.UseHttps(httpsOptions);
                    });
#endif
            });

            var app = builder.Build();

            app.UseExceptionHandler("/Error/InternalError");

            if (Log.IsEnabled(LogEventLevel.Debug))
            {
                app.UseRequestLogging();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseWebSockets();
            app.MapWebSocketManager("/notifications", app.Services.GetService<NotificationsMessageHandler>());

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=OperatingModeSelection}/{action=Index}/{id?}");

                routes.MapRoute("resources", "{*path}",
                    new { controller = "Ressources", action = "Get", path = string.Empty });
            });

            var doneEvent = new ManualResetEventSlim(false);
            using (var cts = new CancellationTokenSource())
            {
                AttachCtrlcSigtermShutdown(cts, doneEvent);

                try
                {
                    app.Start();
                    Log.Information("Using backend port: {0}", backendServerPort);
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to start backend: {0}", ex.Message);
                    Console.WriteLine($"##### TRUDI-BACKEND-ERROR: {ex.Message} #####");
                    doneEvent.Set();
                    return;
                }

                // If the webhost is listening on the port: send it to the Electron frontend
                Console.WriteLine($"##### TRUDI-BACKEND-PORT: {backendServerPort} #####");

                cts.Token.WaitHandle.WaitOne();
                doneEvent.Set();
            }
        }

        private static void AttachCtrlcSigtermShutdown(CancellationTokenSource cts, ManualResetEventSlim resetEvent)
        {
            void Shutdown()
            {
                if (!cts.IsCancellationRequested)
                {
                    try
                    {
                        cts.Cancel();
                    }
                    catch (ObjectDisposedException)
                    {
                    }
                }

                // Wait on the given reset event
                resetEvent.Wait();
            }

            AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) => Shutdown();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Shutdown();

                // Don't terminate the process immediately, wait for the Main thread to exit gracefully.
                eventArgs.Cancel = true;
            };
        }

        /// <summary>
        /// Finds a free TCP port.
        /// </summary>
        /// <returns>The TCP port that was found.</returns>
        public static int FindFreeTcpPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
