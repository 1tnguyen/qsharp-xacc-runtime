using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Quantum.Simulation.Common;
using Microsoft.Quantum.Simulation.Core;

namespace Xacc 
{
    /// <summary>
    /// The XACC Ir adapter simulator class.
    /// Listens to Q# instruction/gate calls to construct XACC IR.
    /// </summary>
    public class IrAdapterSimulator : SimulatorBase, IDisposable
    {
        /// <summary>
        /// Flag to indicate that the full syntax tree has been flattened,
        /// i.e., code paths that depend on measurement results have been resolved.
        /// </summary>
        public bool flattenComplete { get; private set; }
        
        /// <summary>
        /// Name of the XACC backend to execute the Q# code.
        /// Convention: <Platform>:<Device>; where <Device> is optional.
        /// e.g. IBM:paris; tnqvm
        /// </summary>
        public string backendName { get; private set; } 
        
        /// <summary>
        /// The root-level composite instruction representing the quantum-kernel execution.
        /// </summary>
        public CompositeInstruction rootComposite { get; private set; } 

        /// <summary>
        /// Constructs a XACC Ir adapter simulator instance.
        /// </summary>
        // TODO: Implement Qubit manager
        public IrAdapterSimulator(string in_backendName): base(new QubitManager())
        {
            backendName = in_backendName;
            rootComposite = new CompositeInstruction("root");
        }

        /// <summary>
        /// The name of an instance of this simulator.
        /// </summary>
        public override string Name
        {
            get
            {
                return "XACC IR Adapter Simulator";
            }
        }

        /// <summary>
        /// Adds an Instruction to the composite as operations are dispatched to the IQuantumProcessor.
        /// </summary>
        public void AddInstruction(IInstruction in_instruction)
        {
            // TODO: determines the composite to add based on measurement branching.
            rootComposite.addInstruction(in_instruction);
        }                

        /// <summary>
        /// Entry Point
        /// </summary>
        public override Task<O> Run<T, I, O>(I args)
        {
            return Task<O>.Run(() => Execute<T, I, O>(args));
        }

#region Intrinsic Gate Impl
        public class H : Microsoft.Quantum.Intrinsic.H
        {
            IrAdapterSimulator m_adapter;
            public H(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("H", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrlQubits, targetQubit) = args;
                if (ctrlQubits.Length == 1)
                {
                    // CH = Ry(pi/4) - CX - Ry(-pi/4)
                    // i.e. change the basis of the target qubit
                    var ryP = new IntrinsicGate("Ry", targetQubit.Id, Math.PI/4);
                    m_adapter.AddInstruction(ryP);
                    
                    var qubitIds = new List<int>() { ctrlQubits[0].Id, targetQubit.Id };
                    var cnot = new IntrinsicGate("CX", qubitIds);
                    m_adapter.AddInstruction(cnot);
                    
                    var ryM = new IntrinsicGate("Ry", targetQubit.Id, -Math.PI/4);
                    m_adapter.AddInstruction(ryM);
                }
                else
                {
                    // Multi-control: Not developed yet.
                    throw new NotImplementedException();
                }

                return QVoid.Instance;
            };
        }


        public class X : Microsoft.Quantum.Intrinsic.X
        {
            IrAdapterSimulator m_adapter;

            public X(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("X", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrlQubits, targetQubit) = args;
                if (ctrlQubits.Length == 1)
                {
                    var qubitIds = new List<int>() { ctrlQubits[0].Id, targetQubit.Id };
                    var instruction = new IntrinsicGate("CX", qubitIds);
                    m_adapter.AddInstruction(instruction);
                }
                else
                {
                    // Multi-control: Not developed yet.
                    throw new NotImplementedException();
                }

                return QVoid.Instance;
            };
        }


        public class Y : Microsoft.Quantum.Intrinsic.Y
        {
            IrAdapterSimulator m_adapter;

            public Y(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("Y", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrlQubits, targetQubit) = args;
                if (ctrlQubits.Length == 1)
                {
                    // CY = Sdg - CX - S
                    // i.e. change the basis of the target qubit
                    var sdg = new IntrinsicGate("Sdg", targetQubit.Id);
                    m_adapter.AddInstruction(sdg);
                    
                    var qubitIds = new List<int>() { ctrlQubits[0].Id, targetQubit.Id };
                    var cnot = new IntrinsicGate("CX", qubitIds);
                    m_adapter.AddInstruction(cnot);
                    
                    var s = new IntrinsicGate("S", targetQubit.Id);
                    m_adapter.AddInstruction(s);
                }
                else
                {
                    // Multi-control: Not developed yet.
                    throw new NotImplementedException();
                }

                return QVoid.Instance;
            };
        }

        public class Z : Microsoft.Quantum.Intrinsic.Z
        {
            IrAdapterSimulator m_adapter;

            public Z(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("Z", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                var (ctrlQubits, targetQubit) = args;
                if (ctrlQubits.Length == 1)
                {
                    // CZ = H - CX - H
                    // i.e. change the basis of the target qubit
                    var hadamard = new IntrinsicGate("H", targetQubit.Id);
                    m_adapter.AddInstruction(hadamard);
                    
                    var qubitIds = new List<int>() { ctrlQubits[0].Id, targetQubit.Id };
                    var cnot = new IntrinsicGate("CX", qubitIds);
                    m_adapter.AddInstruction(cnot);

                    m_adapter.AddInstruction(hadamard);
                }
                else
                {
                    // Multi-control: Not developed yet.
                    throw new NotImplementedException();
                }

                return QVoid.Instance;
            };
        }

        public class T : Microsoft.Quantum.Intrinsic.T
        {
            IrAdapterSimulator m_adapter;

            public T(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("T", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            /// Applies T-dagger
            public override Func<Qubit, QVoid> AdjointBody => (q1) =>
            {                
                var instruction = new IntrinsicGate("Tdg", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                // TODO
                throw new NotImplementedException();
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledAdjointBody => (_args) =>
            {
                (IQArray<Qubit> ctrls, Qubit q1) = _args;
                throw new NotImplementedException();
                return QVoid.Instance;
            };
        }

        public class S : Microsoft.Quantum.Intrinsic.S
        {
            IrAdapterSimulator m_adapter;

            public S(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                var instruction = new IntrinsicGate("S", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            /// Applies S-dagger
            public override Func<Qubit, QVoid> AdjointBody => (q1) =>
            {                
                var instruction = new IntrinsicGate("Sdg", q1.Id);
                m_adapter.AddInstruction(instruction);
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                throw new NotImplementedException();
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledAdjointBody => (_args) =>
            {
                throw new NotImplementedException();
                (IQArray<Qubit> ctrls, Qubit q1) = _args;
                return QVoid.Instance;
            };
        }

        public class R : Microsoft.Quantum.Intrinsic.R
        {
            IrAdapterSimulator m_adapter;

            public R(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<(Pauli, double, Qubit), QVoid> Body => (_args) =>
            {
                var (basis, angle, q1) = _args;
                switch (basis)
                {
                case Pauli.PauliX:
                {
                    var instruction = new IntrinsicGate("Rx", q1.Id, angle);
                    m_adapter.AddInstruction(instruction);
                    break;
                }   
                case Pauli.PauliY:
                {
                    var instruction = new IntrinsicGate("Ry", q1.Id, angle);
                    m_adapter.AddInstruction(instruction);
                    break;
                }
                case Pauli.PauliZ:
                {
                    var instruction = new IntrinsicGate("Rz", q1.Id, angle);
                    m_adapter.AddInstruction(instruction);
                    break;
                }
                case Pauli.PauliI:
                    break;
                default:
                    throw new InvalidOperationException();
                }
                
                return QVoid.Instance;
            };

            public override Func<(Pauli, double, Qubit), QVoid> AdjointBody => (_args) =>
            {
                var (basis, angle, q1) = _args;
                return this.Body.Invoke((basis, -angle, q1));
            };

            public override Func<(IQArray<Qubit>, (Pauli, double, Qubit)), QVoid> ControlledBody => (_args) =>
            {
                var (ctrlQubits, (basis, angle, targetQubit)) = _args;
                
                if (ctrlQubits.Length == 1)
                {
                    // CRn(theta) = Rn(theta/2) - CX - Rn(-theta/2) - CX
                    var qubitIds = new List<int>() { ctrlQubits[0].Id, targetQubit.Id };
                    var cnot = new IntrinsicGate("CX", qubitIds);
                    switch (basis)
                    {
                    case Pauli.PauliX:
                    {
                        m_adapter.AddInstruction(new IntrinsicGate("Rx", targetQubit.Id, angle/2));
                        m_adapter.AddInstruction(cnot);
                        m_adapter.AddInstruction(new IntrinsicGate("Rx", targetQubit.Id, -angle/2));
                        m_adapter.AddInstruction(cnot);
                        break;
                    }   
                    case Pauli.PauliY:
                    {
                        m_adapter.AddInstruction(new IntrinsicGate("Ry", targetQubit.Id, angle/2));
                        m_adapter.AddInstruction(cnot);
                        m_adapter.AddInstruction(new IntrinsicGate("Ry", targetQubit.Id, -angle/2));
                        m_adapter.AddInstruction(cnot);
                        break;
                    }
                    case Pauli.PauliZ:
                    {
                        m_adapter.AddInstruction(new IntrinsicGate("Rz", targetQubit.Id, angle/2));
                        m_adapter.AddInstruction(cnot);
                        m_adapter.AddInstruction(new IntrinsicGate("Rz", targetQubit.Id, -angle/2));
                        m_adapter.AddInstruction(cnot);
                        break;
                    }
                    case Pauli.PauliI:
                        break;
                    default:
                        throw new InvalidOperationException();
                    }
                }
                else
                {
                    // Multi-control: Not developed yet.
                    throw new NotImplementedException();
                }

                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, (Pauli, double, Qubit)), QVoid> ControlledAdjointBody => (_args) =>
            {
                var (ctrls, (basis, angle, q1)) = _args;
                return this.ControlledBody.Invoke((ctrls, (basis, -angle, q1)));
            };
        }

        public class Measure : Microsoft.Quantum.Intrinsic.Measure
        {
            IrAdapterSimulator m_adapter;
            
            public Measure(IrAdapterSimulator m) : base(m)
            {
                m_adapter = m;
            }

            public override Func<(IQArray<Pauli>, IQArray<Qubit>), Result> Body => (_args) =>
            {
                return Extensions.ToResult(0);
            };
        }
#endregion  

        /// <summary>
        /// IDisposable impl.
        /// </summary>
        public void Dispose()
        {
            // DEBUG
            Console.WriteLine(rootComposite.toString());
        }
    }
}