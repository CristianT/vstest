// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.TestPlatform.PlatformAbstractions
{
    using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
    using System;

    /// <inheritdoc />
    public class PlatformEnvironment : IEnvironment
    {
        /// <inheritdoc />
        public PlatformArchitecture Architecture
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public PlatformOperatingSystem OperatingSystem
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
