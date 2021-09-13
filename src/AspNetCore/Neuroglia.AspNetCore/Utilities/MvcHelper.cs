﻿/*
 * Copyright © 2021 Neuroglia SPRL. All rights reserved.
 * <p>
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * <p>
 * http://www.apache.org/licenses/LICENSE-2.0
 * <p>
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microsoft.AspNetCore
{

    /// <summary>
    /// Defines helpers for MVC
    /// </summary>
    public static class MvcHelper
    {

        /// <summary>
        /// Gets the name of the specified <see cref="ControllerBase"/>
        /// </summary>
        /// <typeparam name="TController">The type of <see cref="ControllerBase"/> to get the name for</typeparam>
        /// <returns>The name of the specified <see cref="ControllerBase"/></returns>
        public static string NameOf<TController>()
            where TController : ControllerBase
        {
            return typeof(TController).Name.Replace("controller", "", StringComparison.InvariantCultureIgnoreCase);
        }

    }

}
