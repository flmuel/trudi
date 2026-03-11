# Creating tag and release 1.7.1-macos.1

## Option A: Include your current macOS changes in the release

```bash
# 1. Stage and commit your macOS support changes
git add .envrc shell.nix src/build-macos.sh
git add .gitignore README.md src/TRuDI.Backend/TRuDI.Backend.csproj src/TRuDI.Frontend/main.js
# Add private-packages only if you intend to ship them
# git add private-packages/

git commit -m "Add macOS build support (arm64/x64)"

# 2. Push to main
git push origin main
```

Then continue with **Create the tag** below (on the new commit).

---

## Option B: Tag the current commit (no new commit)

Skip the commit step and run the tag commands below on current HEAD.

---

## Create the tag

```bash
# Create an annotated tag (recommended – stores message and signer)
git tag -a v1.7.1-macos.1 -m "Release 1.7.1-macos.1: macOS build support (arm64/x64)"

# Push the tag to GitHub
git push origin v1.7.1-macos.1
```

---

## Create the GitHub release

1. Open: **https://github.com/flmuel/trudi/releases/new**
2. Under "Choose a tag", select **v1.7.1-macos.1** (or type it).
3. Set **Release title** to e.g. `1.7.1-macos.1` or `TRuDI 1.7.1 – macOS`.
4. Add release notes (e.g. macOS support, build instructions).
5. Attach any build artifacts (e.g. `.dmg` or `.zip` from `src/build-macos.sh`).
6. Click **Publish release**.

---

## One-liner (tag only, after you’ve committed)

```bash
git tag -a v1.7.1-macos.1 -m "Release 1.7.1-macos.1: macOS build support" && git push origin v1.7.1-macos.1
```
