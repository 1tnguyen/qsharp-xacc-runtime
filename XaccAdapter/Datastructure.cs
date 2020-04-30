using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Xacc
{
    public class InstructionParameter : IConvertible 
    {
        object m_val;

        public InstructionParameter(): this(null) 
        {}

        public InstructionParameter(object value) 
        {
            this.m_val = value;
        }

        #region Conversions
        public static implicit operator InstructionParameter(string v) 
        {
            return new InstructionParameter(v);
        }

        public static implicit operator string(InstructionParameter v) 
        {
            return v == null ? null : v.ToString();
        }

        public static implicit operator InstructionParameter(int? v) 
        {
            return new InstructionParameter(v);
        }

        public static implicit operator int(InstructionParameter v) 
        {
            int o;
            int.TryParse(v, out o);
            return o;
        }

        public static implicit operator int?(InstructionParameter v) 
        {
            int o;
            
            if (int.TryParse(v, out o))
            {
                return o;
            }

            return null;
        }

        public static implicit operator InstructionParameter(float? v) 
        {
            return new InstructionParameter(v);
        }

        public static implicit operator float(InstructionParameter v) 
        {
            float o;
            float.TryParse(v, out o);
            return o;
        }

        public static implicit operator float?(InstructionParameter v) 
        {
            float o;
            if(float.TryParse(v, out o))
            {
                return o;
            }

            return null;
        }

        public static implicit operator InstructionParameter(double? v) 
        {
            return new InstructionParameter(v);
        }

        public static implicit operator double(InstructionParameter v) 
        {
            double o;
            double.TryParse(v, out o);
            return o;
        }
        public static implicit operator double?(InstructionParameter v) 
        {
            double o;
            if(double.TryParse(v, out o))
            {
                return o;
            }
            return null;
        }
        #endregion

        #region IConvertible

        public TypeCode GetTypeCode() 
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider) 
        {
            return false;
        }

        byte IConvertible.ToByte(IFormatProvider provider) 
        {
            return 0;
        }

        char IConvertible.ToChar(IFormatProvider provider) 
        {
            return '\0';
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider) 
        {
            return DateTime.Now;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider) 
        {
            return 0.0M;
        }

        short IConvertible.ToInt16(IFormatProvider provider) 
        {
            return 0;
        }

        int IConvertible.ToInt32(IFormatProvider provider) 
        {
            return this;
        }

        long IConvertible.ToInt64(IFormatProvider provider) 
        {
            return this;
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider) 
        {
            return 0;
        }

        float IConvertible.ToSingle(IFormatProvider provider) 
        {
            return this;
        }

        string IConvertible.ToString(IFormatProvider provider) 
        {
            return this;
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider) 
        {
            return Convert.ChangeType(this, conversionType);
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider) 
        {
            return 0;
        }

        uint IConvertible.ToUInt32(IFormatProvider provider) 
        {
            return 0;
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider) 
        {
            return 0;
        }

        double IConvertible.ToDouble(IFormatProvider provider) 
        {
            return this;
        }
        #endregion

        public static bool operator ==(InstructionParameter a, InstructionParameter b) 
        {
            bool na = (object)a == null || a.m_val == null, nb = (object)b == null || b.m_val == null;
            return na && nb || (!na && !nb && a.m_val.ToString() == b.m_val.ToString());
        }

        public static bool operator !=(InstructionParameter a, InstructionParameter b) 
        {
            return !(a == b);
        }

        public override bool Equals(object obj) 
        {
            return this == (InstructionParameter)obj;
        }

        public override int GetHashCode() 
        {
            return base.GetHashCode();
        }

        public override string ToString() 
        {
            return m_val + "";
        }
    }
    
    
    public interface IInstruction
    {
        IEnumerable<int> bits();
        void setBits(IEnumerable<int> in_bits);
        InstructionParameter getParameter(int in_idx);
        IEnumerable<InstructionParameter> getParameters();
        void setParameter(int in_idx, InstructionParameter in_instParam);
        bool isComposite();
        string toString();
    }

    public class IntrinsicGate : IInstruction
    {
        List<int> m_bits;
        List<InstructionParameter> m_params;
        string m_name;

        public IntrinsicGate(string in_gateName, int in_qbitIdx)
        {
            m_name = in_gateName;
            m_bits = new List<int>() { in_qbitIdx };
            m_params = new List<InstructionParameter>();
        }

        public IntrinsicGate(string in_gateName, IEnumerable<int> in_qbitIdx)
        {
            m_name = in_gateName;
            m_bits = in_qbitIdx.ToList();
            m_params = new List<InstructionParameter>();
        }

        public IntrinsicGate(string in_gateName, int in_qbitIdx, double in_param)
        {
            m_name = in_gateName;
            m_bits = new List<int>() { in_qbitIdx };
            m_params = new List<InstructionParameter>() { in_param };
        }

        public virtual IEnumerable<int> bits()
        {
            return m_bits;
        }

        public virtual void setBits(IEnumerable<int> in_bits)
        {
            m_bits = in_bits.ToList();
        }

        public virtual InstructionParameter getParameter(int in_idx)
        {
            if (in_idx < m_params.Count)
            {
                return m_params[in_idx];
            }
            else
            {
                throw new ArgumentOutOfRangeException("in_idx", "Instruction Parameter List.");
            }
        }

        public virtual IEnumerable<InstructionParameter> getParameters()
        {
            return m_params;
        }

        public virtual void setParameter(int in_idx, InstructionParameter in_instParam)
        {
            if (in_idx < m_params.Count)
            {
                m_params[in_idx] = in_instParam;
            }
            else
            {
                throw new ArgumentOutOfRangeException("in_idx", "Instruction Parameter List.");
            }
        }

        public virtual bool isComposite()
        {
            return false;
        }

        public virtual string toString()
        {
            var gateStr = new StringBuilder(m_name);

            foreach(var param in m_params)
            {
                gateStr.Append(" " + param.ToString());
            }

            foreach (var bit in m_bits)
            {
                gateStr.Append(" q" + bit.ToString());
            }        
                        
            return gateStr.ToString();
        }
    }

    public class CompositeInstruction : IInstruction
    {
        List<int> m_bits;
        List<string> m_args;
        string m_name;
        List<IInstruction> m_instructions;

        public CompositeInstruction(string in_compositeName)
        {
            m_name = in_compositeName;
            m_bits = new List<int>();
            m_args = new List<string>();
            m_instructions = new List<IInstruction>();
        }

        public void addInstruction(IInstruction in_instruction)
        {
            m_instructions.Add(in_instruction);
        }

        public int nInstructions()
        {
            return m_instructions.Count();
        }

        public IEnumerable<IInstruction> getInstructions()
        {
            return m_instructions;
        }

        public virtual IEnumerable<int> bits()
        {
            return m_bits;
        }

        public virtual void setBits(IEnumerable<int> in_bits)
        {
            m_bits = in_bits.ToList();
        }

        public virtual InstructionParameter getParameter(int in_idx)
        {
            if (in_idx < m_args.Count)
            {
                return m_args[in_idx];
            }
            else
            {
                throw new ArgumentOutOfRangeException("in_idx", "Instruction Parameter List.");
            }
        }

        public virtual IEnumerable<InstructionParameter> getParameters()
        {
            var instParams = new List<InstructionParameter>();
            foreach (var arg in m_args)
            {
                instParams.Add(arg);
            }
            return instParams;
        }

        public virtual void setParameter(int in_idx, InstructionParameter in_instParam)
        {
            if (in_idx < m_args.Count)
            {
                m_args[in_idx] = in_instParam.ToString();
            }
            else
            {
                throw new ArgumentOutOfRangeException("in_idx", "Instruction Parameter List.");
            }
        }

        public virtual bool isComposite()
        {
            return true;
        }

        public virtual string toString()
        {
            var compositeStr = new StringBuilder(m_name);
            compositeStr.Append(" {\n");
            foreach (var inst in m_instructions)
            {
                compositeStr.Append(inst.toString());
                compositeStr.Append("\n");
            }            
            
            compositeStr.Append("}\n");
            return compositeStr.ToString();
        }
    }
}