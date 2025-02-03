﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnzymkinetikAddIn.Forms;

namespace EnzymkinetikAddIn.Interfaces
{
    internal interface IFormGenerator
    {
        BaseForm GenerateForm(string concentration, string unit);
    }
}
