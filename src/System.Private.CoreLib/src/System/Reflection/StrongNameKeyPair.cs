// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

/*============================================================
**
**
** 
**
**
** Purpose: Encapsulate access to a public/private key pair
**          used to sign strong name assemblies.
**
**
===========================================================*/

using System;
using System.Runtime.Serialization;

namespace System.Reflection
{
    [Serializable]
    public class StrongNameKeyPair : IDeserializationCallback, ISerializable
    {
        private bool _keyPairExported;
        private byte[] _keyPairArray;
        private String _keyPairContainer;
        private byte[] _publicKey;

        // Build key pair from byte array in memory.
        public StrongNameKeyPair(byte[] keyPairArray)
        {
            if (keyPairArray == null)
                throw new ArgumentNullException(nameof(keyPairArray));

            _keyPairArray = new byte[keyPairArray.Length];
            Array.Copy(keyPairArray, _keyPairArray, keyPairArray.Length);

            _keyPairExported = true;
        }

        protected StrongNameKeyPair(SerializationInfo info, StreamingContext context)
        {
            _keyPairExported = (bool)info.GetValue("_keyPairExported", typeof(bool));
            _keyPairArray = (byte[])info.GetValue("_keyPairArray", typeof(byte[]));
            _keyPairContainer = (string)info.GetValue("_keyPairContainer", typeof(string));
            _publicKey = (byte[])info.GetValue("_publicKey", typeof(byte[]));
        }

        public StrongNameKeyPair(String keyPairContainer)
        {
            throw new PlatformNotSupportedException();
        }

        public byte[] PublicKey
        {
            get
            {
                throw new PlatformNotSupportedException();
            }
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_keyPairExported", _keyPairExported);
            info.AddValue("_keyPairArray", _keyPairArray);
            info.AddValue("_keyPairContainer", _keyPairContainer);
            info.AddValue("_publicKey", _publicKey);
        }

        void IDeserializationCallback.OnDeserialization(Object sender) { }
    }
}
