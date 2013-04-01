//    Copyright 2010 Clinton Sheppard <sheppard@cs.unm.edu>
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;

using FluentWebUITesting;

using OpenQA.Selenium;



namespace decuit.Tests.Integration
{
	public class IntegrationTestRunner
	{
		public void Run(IEnumerable<Action<IWebDriver>> browserActions, string initialPage)
		{
			var baseUrl = new FileInfo(Path.Combine("Pages", initialPage)).FullName;
			UITestRunner.InitializeBrowsers(x =>
				{
					x.CloseBrowserAfterEachTest = false;
					x.UseInternetExplorer = false;
					x.UseFireFox = false;
					x.UseChrome = true;
					x.BaseUrl = baseUrl;
				});

			try
			{
				UITestRunner.RunTest(baseUrl,
				                     "",
				                     browserActions);
			}
			finally
			{
				try
				{
					UITestRunner.CloseBrowsers();
				}
				catch
				{
				}
			}
		}
	}
}