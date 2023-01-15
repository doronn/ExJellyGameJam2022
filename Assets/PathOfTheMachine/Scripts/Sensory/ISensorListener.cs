using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sensory
{
    public interface ISensorListener
    {
        float[] SensoryData { get; }
        void Init(int sensoryDataSize, ISensoryMetaData sensoryMetaData);
    }
}