using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameJamKit.Scripts.Utils.Attributes;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

namespace AiBrain
{
    public class BrainsManager : MonoBehaviour
    {
        [SerializeField]
        private List<Brain> _brains;

        [SerializeField]
        private BrainManagerConfigurations _brainConfig; 

        [SerializeField]
        private ParentConstraint _constraint;

        [SerializeField]
        private TMP_Text _iterationCounterText;

        [SerializeField]
        private Transform _cameraTransform;

        private string _bestBrainJson = null;

        private static int _generationCounter = 0;

        private float _startTime = 0;
        private NeuralNetworkArray _bestNetworks;

        private void Awake()
        {
            Application.runInBackground = true;
            if (PlayerPrefs.HasKey(_brainConfig.BrainId))
            {
                _bestBrainJson = PlayerPrefs.GetString(_brainConfig.BrainId);
                _generationCounter = PlayerPrefs.GetInt(_brainConfig.BrainId + "_Counter", 0);
            }
        }

        public void RegisterBrain(Brain brain)
        {
            _brains.Add(brain);
        }

        public async UniTaskVoid StartSimulation()
        {
            _bestNetworks = string.IsNullOrEmpty(_bestBrainJson) ? null : JsonConvert.DeserializeObject<NeuralNetworkArray>(_bestBrainJson);
            var copiedNetworksCount = _bestNetworks?.Networks?.Length ?? 0;
            for (var index = 0; index < _brains.Count; index++)
            {
                var brain = _brains[index];
                if (index < copiedNetworksCount)
                {
                    if (_bestNetworks?.Networks != null)
                    {
                        var neuralNetworkSaveObject = _bestNetworks.Networks[index];
                        brain.Init(neuralNetworkSaveObject.Network, _brainConfig.MutationAmount);
                        continue;
                    }
                }
                var networkToCopy = _bestNetworks?.Networks?[index % copiedNetworksCount];
                var copiedNetwork = JsonConvert.DeserializeObject<NeuralNetworkSaveObject>(JsonConvert.SerializeObject(networkToCopy));
                var nextNetwork = copiedNetworksCount > 0 ? copiedNetwork?.Network : null;
                var isMutated = false;
                if (nextNetwork != null)
                {
                    NeuralNetwork.Mutate(nextNetwork, _brainConfig.MutationAmount);
                    isMutated = true;
                }

                brain.Init(nextNetwork, _brainConfig.MutationAmount, isMutated: isMutated);
                
                if (index % 5 == 0)
                {
                    await UniTask.DelayFrame(1, cancellationToken: this.GetCancellationTokenOnDestroy());
                }
            }

            for (var index = 0; index < _brains.Count; index++)
            {
                var brain = _brains[index];
                brain.SetAsInitialized();
            }

            if (_brainConfig.AutoLearn)
            {
                LearnEndTimer().Forget();
            }

            FollowBestBrain().Forget();
        }

        private async UniTaskVoid FollowBestBrain()
        {
            var cancellationTokenOnDestroy = this.GetCancellationTokenOnDestroy();
            while (!cancellationTokenOnDestroy.IsCancellationRequested)
            {
                if (Brain.BestBrain)
                {
                    _cameraTransform.SetParent(Brain.BestBrain.transform.parent, false);
                }

                await UniTask.Delay(100, cancellationToken: cancellationTokenOnDestroy);
            }
        }

        private async UniTaskVoid LearnEndTimer()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_brainConfig.IterationTime), cancellationToken: this.GetCancellationTokenOnDestroy());

            _generationCounter++;
            PlayerPrefs.SetInt(_brainConfig.BrainId + "_Counter", _generationCounter);
            SaveBestBrain();
            ResetSimulation();
        }

        [Button]
        public void SaveBestBrain()
        {
            var bestBrains = _brains;

            if (bestBrains == null || bestBrains.Count == 0)
            {
                throw new Exception("No brain found ???");
            }

            var bestNetworksSoFar =
                bestBrains.Select(brain =>
                    new NeuralNetworkSaveObject
                    {
                        Network = brain.Network,
                        NetworkScore = brain.TotalScore
                    }).ToList();
            /*if (_bestNetworks?.Networks != null)
            {
                bestNetworksSoFar.AddRange(_bestNetworks.Networks);
            }*/

            var orderedNetworks =
                bestNetworksSoFar.OrderBy(networkSaveObject => networkSaveObject.NetworkScore).Distinct(NeuralNetworkSaveObjectEqualityComparer.Instance);
            var takenBestBrains = orderedNetworks.Take(_brainConfig.AmountOfSavedNetworks);
            var bestNetworks = takenBestBrains as NeuralNetworkSaveObject[] ?? takenBestBrains.ToArray();

            var brainJson = JsonConvert.SerializeObject(new NeuralNetworkArray { Networks = bestNetworks });
            var averageBestScores = bestNetworks.Average(network => network.NetworkScore);

            var brainBestAvgScore = _brainConfig.BrainId + "_bestAvgScore";
            var lastKnownBestAverage = PlayerPrefs.GetFloat(brainBestAvgScore, 0);
            var highestTotalScore = bestNetworks.FirstOrDefault()?.NetworkScore;

            // if (highestTotalScore > 10)
            // {
                Debug.Log(
                    $"Saving best brains with Maximum of {highestTotalScore.ToString()} and avg of {averageBestScores.ToString()}. last average was {lastKnownBestAverage.ToString()}\nIndices: {string.Join(", ", bestNetworks.Select(o => o.Network.NetworkIndex))}");
                PlayerPrefs.SetFloat(brainBestAvgScore, averageBestScores);
                PlayerPrefs.SetString(_brainConfig.BrainId, brainJson);
            // }
            // else
            // {
                // Debug.Log(
                    // $"NOT saving best brains with Maximum of {highestTotalScore.ToString()} and avg of {averageBestScores.ToString()}. last average was {lastKnownBestAverage.ToString()}\nIndices: {string.Join(", ", bestNetworks.Select(o => o.Network.NetworkIndex))}");
            // }

            PlayerPrefs.Save();
        }

        private Brain GetBestBrain()
        {
            if (_brains.Count == 0 || !_brains.Any(IsBrainInitialized))
            {
                return null;
            }
            
            var maxBrainScore = _brains.Where(IsBrainInitialized).Max(BrainScore);
            var bestBrain = _brains.FirstOrDefault(brain => brain.TotalScore == maxBrainScore);
            return bestBrain;
        }

        private float BrainScore(Brain brain)
        {
            return brain.TotalScore;
        }

        private bool IsBrainInitialized(Brain brain)
        {
            return brain.IsInitialized;
        }

        private List<Brain> GetBestBrains()
        {
            var sortedBrains = _brains.OrderByDescending(brain => brain.TotalScore).ToList();
            sortedBrains.FirstOrDefault()?.SetAsBest();
            return sortedBrains;
        }

        [Button]
        public void ClearBestBrain()
        {
            PlayerPrefs.DeleteKey(_brainConfig.BrainId);
        }

        [Button]
        public void ResetSimulation()
        {
            StopAllCoroutines();
            SceneManager.LoadScene(0);
        }

        private void OnDestroy()
        {
            
        }
    }
}