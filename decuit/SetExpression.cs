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
using System.Linq;

using FluentWebUITesting.Controls;

using OpenQA.Selenium;

namespace gar3t.decuit
{
	public abstract class SetExpression
	{
		protected IWebDriver Browser { get; private set; }
		private static readonly List<IInputSetter> _setters = new List<IInputSetter>
		                                                      {
			                                                      new TextBoxSetter(),
			                                                      new CheckBoxSetter(),
			                                                      new DropDownListSetter(),
																  new RadioButtonOptionSetter()
		                                                      };

		protected SetExpression(IWebDriver browser)
		{
			Browser = browser;
		}

		public abstract ControlWrapperBase Control { get; }

		public WaitWrapper To(string text)
		{
			var control = Control;
			control.Exists().ShouldBeTrue();
			control.Enabled().ShouldBeTrue();

			var setter = _setters.FirstOrDefault(x => x.IsMatch(control));
			if (setter == null)
			{
				throw new ArgumentOutOfRangeException("text", String.Format("There is no configured InputSetter for {0} ", control.HowFound));
			}
			return setter.SetTo(control, text);
		}

	}
}