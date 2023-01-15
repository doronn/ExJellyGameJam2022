using System;
using System.Collections.Generic;
using UnityEngine;

namespace AiBrain
{
    public class Brain : MonoBehaviour
    {
        private const string SCORE_TEXT_FORMAT = "{0}\n{1}";
        private static int _bestId = -1;
        private static int _networkIndex = 100;
        private static float _highestBrainScore = float.MaxValue;
        public static Brain BestBrain = null;
        private IBrainInputListener _brainInputListener;
        private IGetSensoryData _sensoryData;
        public NeuralNetwork Network { get; private set; }
        public bool IsInitialized { get; private set; }
        public float TotalScore
        {
            get => _totalScore;
            private set
            {
                var addedValue = value - _totalScore;
                _totalScore = value;
                if (addedValue < _highestBrainScore || BestBrain == this)
                {
                    _highestBrainScore = addedValue;
                    BestBrain = this;
                }

                UpdateCharacterScoreText();
            }
        }

        private void UpdateCharacterScoreText()
        {
            
        }
        
        [SerializeField]
        private float _totalScore = 0;

        private int _sensorCount;

        public void Init(NeuralNetwork predefinedNetwork = null, float brainConfigMutationAmount = 0, int score = 0, bool isMutated = false)
        {
            _highestBrainScore = float.MaxValue;
            BestBrain = null;
            _brainInputListener ??= gameObject.GetComponent<IBrainInputListener>();
            if (_brainInputListener == null)
            {
                throw new Exception("missing input listener");
            }
            
            _sensoryData ??= gameObject.GetComponent<IGetSensoryData>();
            if (_sensoryData == null || _sensoryData.Current.Length == 0)
            {
                throw new Exception("missing sensor");
            }

            _networkIndex = Guid.NewGuid().GetHashCode();
            
            _sensorCount = _sensoryData.Current.Length;
            var neuronCounts = new[] { _sensorCount, _sensorCount / 2, 3 };
            
            Network = predefinedNetwork ?? new NeuralNetwork(_networkIndex, neuronCounts);
            
            if (predefinedNetwork != null)
            {
                _totalScore = score;
                NeuralNetwork.TransformNetworkNeurons(Network, neuronCounts);
                if (isMutated && brainConfigMutationAmount > 0)
                {
                    Network.NetworkIndex = _networkIndex;
                }
            }
            
            // _brainInputListener.Init(OnDied);
        }

        public void SetAsInitialized()
        {
            IsInitialized = true;
        }

        public void SetAsBest()
        {
            _bestId = _networkIndex;
        }

        private bool IsBest()
        {
            return _bestId == _networkIndex;
        }

        private void FixedUpdate()
        {
            if (!IsInitialized)
            {
                return;
            }
            
            if (!_sensoryData.Alive)
            {
                IsInitialized = false;
                return;
            }
            
            var potentialAddedScore = _sensoryData.Score;
            
            var outputs = NeuralNetwork.FeedForward(
                _sensoryData.Current, Network);
            
            _brainInputListener.InputReset();
            for (var i = 0; i < outputs.Length; i++)
            {
                var outputValue = outputs[i];
                if (outputValue > 0)
                {
                    _brainInputListener.RegisterInput(i, outputValue);
                }
            }
            _brainInputListener.ExecuteInputs();

            // if (_brainInputListener.DidReceiveBadInputs())
            // {
                // potentialAddedScore *= 10;
            // }

            TotalScore += potentialAddedScore;
        }
    }
}