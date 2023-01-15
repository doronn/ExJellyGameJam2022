using System;
using Random = System.Random;

namespace AiBrain
{
    [Serializable]
    public class BrainLevel
    {
        public float[] Inputs;
        public float[] Outputs;
        public float[] Biases;
        public float[][] Weights;

        private readonly Random _random;
        public BrainLevel(int inputCount, int outputCount, Random random)
        {
            _random = random;
            Inputs = new float[inputCount];
            Outputs = new float[outputCount];
            Biases = new float[outputCount];

            Weights = new float[inputCount][];
            for (var i = 0; i < inputCount; i++)
            {
                Weights[i] = new float[outputCount];
            }
            
            Randomize(this);
        }

        private static void Randomize(BrainLevel level)
        {
            for (var i = 0; i < level.Inputs.Length; i++)
            {
                for (var j = 0; j < level.Outputs.Length; j++)
                {
                    level.Weights[i][j] = level._random.GetNextRandom();
                }
            }

            for (var i = 0; i < level.Biases.Length; i++)
            {
                level.Biases[i] = level._random.GetNextRandom();
            }
        }

        public static float[] FeedForward(float[] givenInputs, BrainLevel level)
        {
            for (var i = 0; i < level.Inputs.Length; i++)
            {
                try
                {
                    level.Inputs[i] = givenInputs[i];
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            for (var i = 0; i < level.Outputs.Length; i++)
            {
                float sum = 0;
                for (var j = 0; j < level.Inputs.Length; j++)
                {
                    sum += level.Inputs[j] * level.Weights[j][i];
                }

                if (sum > level.Biases[i])
                {
                    level.Outputs[i] = 1;
                }
                else
                {
                    level.Outputs[i] = 0;
                }
            }

            return level.Outputs;
        }

        public static void TransformBrainNeuronCounts(BrainLevel brainLevel, int inputCount, int outputCount)
        {
            var currentInputCount = brainLevel.Inputs.Length;
            var currentOutputCount = brainLevel.Outputs.Length;

            if (currentInputCount == inputCount && currentOutputCount == outputCount)
            {
                return;
            }
            
            var minInputCount = Math.Min(currentInputCount, inputCount);
            var minOutputCount = Math.Min(currentOutputCount, outputCount);

            if (currentInputCount != inputCount)
            {
                ChangeArraySize(ref brainLevel.Inputs, inputCount, minInputCount);
            }

            if (currentOutputCount != outputCount)
            {
                ChangeArraySize(ref brainLevel.Outputs, outputCount, minOutputCount);
                ChangeArraySize(ref brainLevel.Biases, outputCount, minOutputCount);
            }
            

            var updatedArray = new float[inputCount][];
            Array.Copy(brainLevel.Weights, 0, updatedArray, 0, minInputCount);
            brainLevel.Weights = updatedArray;
            
            for (var i = 0; i < inputCount; i++)
            {
                if (brainLevel.Weights[i] == null)
                {
                    brainLevel.Weights[i] = new float[outputCount];
                    continue;
                }
                
                ChangeArraySize(ref brainLevel.Weights[i], outputCount, minOutputCount);
            }
        }

        private static void ChangeArraySize(ref float[] arrayToChange, int newCount, int minCount)
        {
            var updatedArray = new float[newCount];
            Array.Copy(arrayToChange, 0, updatedArray, 0, minCount);
            arrayToChange = updatedArray;
        }
    }
}