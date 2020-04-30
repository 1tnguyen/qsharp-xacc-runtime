using System;
using Microsoft.Quantum.Simulation.Common;
using Microsoft.Quantum.Simulation.Core;
using System.Collections.Generic;
using System.Linq;

namespace Xacc
{
    public class QubitManager: IQubitManager
    {
        private class QubitNonAbstract : Qubit
        {
            // This class is only needed because Qubit is abstract. 
            // It is equivalent to Qubit and adds nothing to it except the ability to create it.
            public QubitNonAbstract(int id) : base(id) { }
        }
        const long MAX_NUMBER_QUBITS = 50;
        long nbQubits; // TODO: handle qubit borrowing, for now, just assume all qubits are newly allocated


        /// <summary>
        /// Ctor.
        /// </summary>
        public QubitManager()
        {
            nbQubits = 0;
        }

        /// <summary>
        /// Allocates a qubit.
        /// Qubit id will be the incremental counter value.
        /// </summary>
        public virtual Qubit Allocate()
        {
            if (nbQubits >= MAX_NUMBER_QUBITS)
            {
                throw new InsufficientMemoryException();
            }
            Console.Write("Allocate new qubit!\n");
            var qubitId = nbQubits;
            nbQubits++;
            return new QubitNonAbstract((int)qubitId);
        }

        /// <summary>
        /// Allocates an array of qubits.
        /// </summary>
        public virtual IQArray<Qubit> Allocate(long count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Attempt to allocate zero qubits.");
            }

            var result = QArray<Qubit>.Create(count);

            for (int i = 0; i < count; i++)
            {
                result.Modify(i, Allocate());
            }

            return result;
        }

        public virtual void Release(Qubit qubit)
        {
            // TODO
        }

        public virtual void Release(IQArray<Qubit> qubits)
        {
            // TODO
        }

        public virtual void Disable(Qubit qubit)
        {
            // TODO
        }

        public virtual void Disable(IQArray<Qubit> qubits)
        {
            // TODO
        }

        public virtual Qubit Borrow()
        {
            // Just returns a new qubit
            return Allocate();
        }

        public virtual IQArray<Qubit> Borrow(long count)
        {
            return Allocate(count);
        }

        public virtual void Return(Qubit q)
        {
            // Nothing to do, not support borrow yet.
        }

        public virtual void Return(IQArray<Qubit> qubits)
        {
            // Nothing
        }

        public virtual bool IsValid(Qubit qubit)
        {
            return qubit.Id < nbQubits;
        }

        public virtual bool IsFree(Qubit qubit)
        {
            return IsValid(qubit);
        }

        public virtual bool IsDisabled(Qubit qubit)
        {
            return false;
        }

        public virtual long GetFreeQubitsCount()
        {
            return MAX_NUMBER_QUBITS - nbQubits;
        }

        public virtual long GetQubitsAvailableToBorrowCount()
        {
            return 0;
        }
        public virtual long GetParentQubitsAvailableToBorrowCount()
        {
            return 0;
        }
        public virtual long GetAllocatedQubitsCount()
        {
            return nbQubits;
        }
        public virtual IEnumerable<long> GetAllocatedIds()
        {
            return Enumerable.Range(0, (int)nbQubits)
                    .Select(i => (long)i)
                    .ToList();
        }

        public virtual bool DisableBorrowing
        {
            get
            {
                return true;
            }

        }

        /// <summary>
        /// Callback to notify QubitManager that an operation execution has started. 
        /// This helps manage qubits scope.
        /// </summary>
        public virtual void OnOperationStart(ICallable operation, IApplyData values)
        {
            // TODO
            Console.Write("OnOperationStart\n");
        }

        /// <summary>
        /// Callback to notify QubitManager that an operation execution has ended. 
        /// This helps manage qubits scope.
        /// </summary>
        public virtual void OnOperationEnd(ICallable operation, IApplyData values)
        {
            // TODO
            Console.Write("OnOperationEnd\n");
        }

        /// <summary>
        /// Returns true if a qubit has been allocated just for borrowing, has been borrowed exactly once,
        /// and thus will be released after it is returned.
        /// </summary>
        public virtual bool ToBeReleasedAfterReturn(Qubit qubit)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Returns a count of input qubits that have been allocated just for borrowing, borrowed exactly once,
        /// and thus will be released after they are returned.
        /// </summary>
        public virtual long ToBeReleasedAfterReturnCount(IQArray<Qubit> qubitsToReturn)
        {
            if (qubitsToReturn == null || qubitsToReturn.Length == 0)
            {
                return 0;
            }

            long count = 0;
            foreach (var qubit in qubitsToReturn)
            {
                if (this.ToBeReleasedAfterReturn(qubit)) 
                { 
                    count++; 
                }
            }
            return count;
         }
    }
}