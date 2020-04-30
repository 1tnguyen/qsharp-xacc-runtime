#pragma once

#include <cstddef>

extern "C" {
void execute(const char* xasmSrc, const char* backendName);
}