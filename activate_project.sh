#!/usr/bin/env bash
# ─────────────────────────────────────────────────────────────────────────────
# Shadow Scan — Project environment activation
# IMPORTANT: must be sourced, not executed directly
# Usage: source activate_project.sh   (or: . activate_project.sh)
# ─────────────────────────────────────────────────────────────────────────────

# Guard: warn if executed instead of sourced
if [[ "${BASH_SOURCE[0]}" == "${0}" ]]; then
    echo "ERROR: run as:  source activate_project.sh"
    exit 1
fi

CONDA_ENV_PATH="/media/fox/envs/envRFID_Reader"
ANDROID_SDK_PATH="/home/fox/Android/Sdk"

# Initialize conda shell functions (required before conda activate)
eval "$(conda shell.bash hook)"
conda activate envRFID_Reader

# Java from conda env (OpenJDK 17)
export JAVA_HOME="$CONDA_ENV_PATH"
export PATH="$JAVA_HOME/bin:$PATH"

# Android SDK
export ANDROID_HOME="$ANDROID_SDK_PATH"
export ANDROID_SDK_ROOT="$ANDROID_SDK_PATH"
export PATH="$ANDROID_HOME/platform-tools:$ANDROID_HOME/tools:$PATH"

echo ""
echo "  Shadow Scan environment activated"
echo "  Java   : $(java -version 2>&1 | head -1)"
echo "  Python : $(python --version)"
echo "  JAVA_HOME   : $JAVA_HOME"
echo "  ANDROID_HOME: $ANDROID_HOME"
echo ""
