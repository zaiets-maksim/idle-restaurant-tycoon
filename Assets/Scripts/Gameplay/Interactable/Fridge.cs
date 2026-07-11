using System;
using System.Collections;
using tetris.Scripts.Extensions;
using UnityEngine;

namespace Interactable
{
    public class Fridge : KitchenItem
    {
        [SerializeField] private Animation _animation;

        private const string OpenDoor = "OpenDoor";
        private const string CloseDoor = "CloseDoor";
        public float DelayAfterClose { get; private set; } = 1f;

        // [SerializeField] private int _foodCount;
        // public bool HasFood => _foodCount > 0;

        public override void Interact()
        {
            StartCoroutine(GetFood());
        }

        private IEnumerator GetFood()
        {
            float randomTime = TimeExtensions.RandomTime(5, 10);
            InteractionTime = randomTime;
            var animationTime = Open();
            yield return new WaitForSeconds(animationTime);
            yield return new WaitForSeconds(randomTime);
            
        
            animationTime = Close();
            yield return new WaitForSeconds(animationTime);
            Release();
        }
        
        private float Open()
        {
            _animation.Play(OpenDoor);
            return _animation[CloseDoor].length;
        }

        private float Close()
        {
            _animation.Play(CloseDoor);
            return _animation[CloseDoor].length;
        }
        
        
        // private IEnumerator GetFood(Action<int> onFoodTaken)
        // {
        //     if (!HasFood)
        //         yield break;
        //
        //     IsOccupied = true;
        //     var animationTime = Open();
        //     yield return new WaitForSeconds(animationTime + 0.5f);
        //
        //     --_foodCount;
        //
        //     animationTime = Close();
        //     yield return new WaitForSeconds(animationTime);
        //     Release();
        // }
        //
        // public IEnumerator PutFood(int food)
        // {
        //     if (HasFood)
        //         yield break;
        //
        //     IsOccupied = true;
        //     var animationTime = Open();
        //     yield return new WaitForSeconds(animationTime + 0.5f);
        //
        //     _foodCount += food;
        //
        //     animationTime = Close();
        //     yield return new WaitForSeconds(animationTime);
        //     Release();
        // }
    }
}
