
#include "capi.hpp"

extern "C" {

// !! TEMP CODE !!
unsigned init()
{
    return 0;
}
  
void destroy(unsigned id)
{
}

void seed(unsigned id, unsigned s)
{
}

size_t random_choice(unsigned id, size_t n, double* p)
{
    return 0;
}

double JointEnsembleProbability(unsigned id, unsigned n, int* b, unsigned* q)
{
    return 0.0;
}


void allocateQubit(unsigned id, unsigned q)
{
}

void release(unsigned id, unsigned q)
{
}

unsigned num_qubits(unsigned id)
{
    return 0;
}
}
