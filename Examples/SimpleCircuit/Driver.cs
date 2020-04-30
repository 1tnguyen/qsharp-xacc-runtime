using System.Threading.Tasks;
using Microsoft.Quantum.Simulation.Simulators;
using Xacc;

namespace SimpleCircuit
{
    class Driver
    {
        static async Task Main(string[] args)
        {
            using var qsim = new IrAdapterSimulator("qcs:Aspen-4-4Q-A");
            await simpleQsharp.Run(qsim);
        }
    }
}