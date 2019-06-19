﻿using System;
using NUnit.Framework;
using SuperMaxim.Messaging;
using UnityEngine;

namespace Tests
{
    public class MessengerLoadTest
    {
        private const int SendMessageChanksCount = 100;

        private const int SendMessagesCount = 100000;

        private int _receivedCount;

        private readonly LoadTestPayload _payload = new LoadTestPayload();

        private class LoadTestPayload
        {

        }

        [Test]
        public void TestLoad()
        {
            Messenger.Default.Subscribe<LoadTestPayload>(OnTestCallback);

            double time = 0.0;
            for (int i = 0; i < SendMessageChanksCount; i++)
            {
                time += TestLoadLoop();
            }

            Debug.LogFormat("Load Test: average time {0}", Math.Round(time / SendMessageChanksCount, 3));
        }

        private double TestLoadLoop()
        {
            _receivedCount = 0;
            var time = DateTime.Now.TimeOfDay;

            for (int i = 0; i < SendMessagesCount; i++)
            {
                Messenger.Default.Publish(_payload);
            }

            time = DateTime.Now.TimeOfDay - time;
            Debug.LogFormat("Load Test: sent {0} messages, received {1} messages, took {2} seconds",
                                SendMessagesCount, _receivedCount, Math.Round(time.TotalSeconds, 3));

            Assert.AreEqual(SendMessagesCount, _receivedCount);

            return time.TotalSeconds;
        }

        private void OnTestCallback(LoadTestPayload payload)
        {
            _receivedCount++;
        }
    }
}