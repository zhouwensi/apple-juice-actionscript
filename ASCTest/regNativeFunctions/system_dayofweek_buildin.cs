﻿using ASBinCode;
using ASBinCode.rtti;
using ASRuntime;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASCTest.regNativeFunctions
{
    class system_dayOfWeek_ctor : NativeFunctionBase
    {
        public system_dayOfWeek_ctor()
        {
            para = new List<RunTimeDataType>();
        }

        public override bool isMethod
        {
            get
            {
                return true;
            }
        }

        public override string name
        {
            get
            {
                return "_system_DayOfWeek_ctor";
            }
        }

        List<RunTimeDataType> para;
        public override List<RunTimeDataType> parameters
        {
            get
            {
                return para;
            }
        }

        public override RunTimeDataType returnType
        {
            get
            {
                return RunTimeDataType.rt_void;
            }
        }

        public override RunTimeValueBase execute(RunTimeValueBase thisObj, SLOT[] argements, object stackframe, out string errormessage, out int errorno)
        {
            errormessage = null; errorno = 0;
            return ASBinCode.rtData.rtUndefined.undefined;

        }
    }

    class system_dayOfWeek_operator_bitOr : ASRuntime.nativefuncs.NativeConstParameterFunction
    {
        public system_dayOfWeek_operator_bitOr() : base(2)
        {
            para = new List<RunTimeDataType>();
            para.Add(RunTimeDataType.rt_void);
            para.Add(RunTimeDataType.rt_void);
        }

        public override bool isMethod
        {
            get
            {
                return true;
            }
        }

        public override string name
        {
            get
            {
                return "_system_DayOfWeek_operator_bitOr";
            }
        }

        List<RunTimeDataType> para;
        public override List<RunTimeDataType> parameters
        {
            get
            {
                return para;
            }
        }

        public override RunTimeDataType returnType
        {
            get
            {
                return RunTimeDataType.rt_void;
            }
        }

        public override void execute3(RunTimeValueBase thisObj, FunctionDefine functionDefine, SLOT returnSlot, SourceToken token, StackFrame stackframe, out bool success)
        {
            DayOfWeek ts1;

            if (argements[0].rtType == RunTimeDataType.rt_null)
            {
                ts1 = default(DayOfWeek);
            }
            else
            {
                LinkObj<object> argObj = (LinkObj<object>)((ASBinCode.rtData.rtObject)argements[0]).value;
                ts1 = (DayOfWeek)argObj.value;
            }

            DayOfWeek ts2;

            if (argements[1].rtType == RunTimeDataType.rt_null)
            {
                ts2 = default(DayOfWeek);
            }
            else
            {
                LinkObj<object> argObj = (LinkObj<object>)((ASBinCode.rtData.rtObject)argements[1]).value;
                ts2 = (DayOfWeek)argObj.value;
            }

            ((StackSlot)returnSlot).setLinkObjectValue(
                bin.getClassByRunTimeDataType(functionDefine.signature.returnType), stackframe.player, ts1 | ts2);

            success = true;
        }
    }


}
