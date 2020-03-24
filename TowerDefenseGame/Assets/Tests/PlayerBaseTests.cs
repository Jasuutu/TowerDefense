using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests
{
    public class PlayerBaseTests
    {
        private PlayerBase playerBase;
        private Text healthText;

        [SetUp]
        public void Setup()
        {
            var prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/PlayerBase.prefab", typeof(PlayerBase));
            var playerBaseGameObject = MonoBehaviour.Instantiate(prefab) as PlayerBase;
            playerBase = playerBaseGameObject.GetComponent<PlayerBase>();
            healthText = playerBase.GetComponentInChildren<Text>();
        }

        [UnityTest]
        public IEnumerator PlayerBaseLosesHealthWhenTakingDamage()
        {
            Assert.AreEqual(playerBase.Health, 100);

            playerBase.TakeDamage(10);

            Assert.AreEqual(playerBase.Health, 90);

            yield return null;
        }

        [UnityTest]
        public IEnumerator PlayBaseHeathTextShouldReflectCurrentTotal()
        {
            Assert.AreEqual(healthText.text, "100 / 100");

            playerBase.TakeDamage(10);

            Assert.AreEqual(healthText.text, "90 / 100");

            yield return new WaitForSeconds(.01f);
        }

        [UnityTest]
        public IEnumerator PlayerBaseDefenseReducesDamage()
        {
            playerBase.Defense = 10;
            playerBase.TakeDamage(20);

            Assert.AreEqual(playerBase.Health, 90);
            
            yield return null;
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(playerBase.gameObject);
        }
    }
}
