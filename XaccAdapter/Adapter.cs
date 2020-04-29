using System;
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
        /// Constructs a XACC Ir adapter simulator instance.
        /// </summary>
        // TODO: Implement Qubit manager
        public IrAdapterSimulator(): base()
        {}

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

#region Intrinsic Gate Impl
        public class H : Microsoft.Quantum.Intrinsic.H
        {
            public H(IrAdapterSimulator m) : base(m)
            {}

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                return QVoid.Instance;
            };
        }


        public class X : Microsoft.Quantum.Intrinsic.X
        {
            public X(IrAdapterSimulator m) : base(m)
            {}

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                return QVoid.Instance;
            };
        }


        public class Y : Microsoft.Quantum.Intrinsic.Y
        {
            public Y(IrAdapterSimulator m) : base(m)
            {}

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                return QVoid.Instance;
            };


            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                return QVoid.Instance;
            };
        }


        public class T : Microsoft.Quantum.Intrinsic.T
        {
            public T(IrAdapterSimulator m) : base(m)
            {}

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                return QVoid.Instance;
            };

            /// Applies T-dagger
            public override Func<Qubit, QVoid> AdjointBody => (q1) =>
            {                
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledAdjointBody => (_args) =>
            {
                (IQArray<Qubit> ctrls, Qubit q1) = _args;
                return QVoid.Instance;
            };
        }

        public class S : Microsoft.Quantum.Intrinsic.S
        {
            public S(IrAdapterSimulator m) : base(m)
            {}

            public override Func<Qubit, QVoid> Body => (q1) =>
            {
                return QVoid.Instance;
            };

            /// Applies S-dagger
            public override Func<Qubit, QVoid> AdjointBody => (q1) =>
            {                
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledBody => (args) =>
            {
                return QVoid.Instance;
            };

            public override Func<(IQArray<Qubit>, Qubit), QVoid> ControlledAdjointBody => (_args) =>
            {
                (IQArray<Qubit> ctrls, Qubit q1) = _args;
                return QVoid.Instance;
            };
        }
        public class R : Microsoft.Quantum.Intrinsic.R
        {
            public R(IrAdapterSimulator m) : base(m)
            {}

            public override Func<(Pauli, double, Qubit), QVoid> Body => (_args) =>
            {
                var (basis, angle, q1) = _args;
                return QVoid.Instance;
            };

            public override Func<(Pauli, double, Qubit), QVoid> AdjointBody => (_args) =>
            {
                var (basis, angle, q1) = _args;
                return this.Body.Invoke((basis, -angle, q1));
            };

            public override Func<(IQArray<Qubit>, (Pauli, double, Qubit)), QVoid> ControlledBody => (_args) =>
            {
                var (ctrls, (basis, angle, q1)) = _args;
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
            public Measure(IrAdapterSimulator m) : base(m)
            {}

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
            // Nothing to do
        }
    }
}