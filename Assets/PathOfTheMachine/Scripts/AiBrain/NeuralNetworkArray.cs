using System;
using System.Collections.Generic;

namespace AiBrain
{
    [Serializable]
    public class NeuralNetworkArray
    {
        public NeuralNetworkSaveObject[] Networks;
    }

    [Serializable]
    public class NeuralNetworkSaveObject
    {
        public float NetworkScore;
        public NeuralNetwork Network;
    }
    
    public class NeuralNetworkSaveObjectEqualityComparer : IEqualityComparer<NeuralNetworkSaveObject>
    {
        private static NeuralNetworkSaveObjectEqualityComparer _instance;

        public static NeuralNetworkSaveObjectEqualityComparer Instance
        {
            get { return _instance ??= new NeuralNetworkSaveObjectEqualityComparer(); }
        }


        private NeuralNetworkSaveObjectEqualityComparer()
        {
            
        }

        public bool Equals(NeuralNetworkSaveObject x, NeuralNetworkSaveObject y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Network.NetworkIndex == y.Network.NetworkIndex;
        }

        public int GetHashCode(NeuralNetworkSaveObject obj)
        {
            return obj.Network.NetworkIndex;
        }
    }
}