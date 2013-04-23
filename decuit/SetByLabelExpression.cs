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
using System.Linq;

using FluentAssert;

using FluentWebUITesting.Controls;
using FluentWebUITesting.Extensions;

using OpenQA.Selenium;

namespace gar3t.decuit
{
	public class SetByLabelExpression : SetExpression
	{
		public SetByLabelExpression(IWebDriver browser, string labelText)
			: base(browser)
		{
			LabelText = labelText;
		}

		public string LabelText { get; private set; }

		private string GetItsLinkedControlId()
		{
			var labels = Browser.Labels();
			var label = labels.FirstOrDefault(x => x.Text().GetValue().Trim() == LabelText.Trim());
			label.ShouldNotBeNull(String.Format("Could not find Label with text '{0}'", LabelText.Trim()));

//// ReSharper disable PossibleNullReferenceException
			string itsLinkedControlId = label.For;
//// ReSharper restore PossibleNullReferenceException
			itsLinkedControlId.ShouldNotBeNullOrEmpty(String.Format("Label with text '{0}' does not have a For attribute", LabelText));
			return itsLinkedControlId;
		}

		public override ControlWrapperBase Control
		{
			get
			{
				string itsLinkedControlId = GetItsLinkedControlId();
				var control = Browser.FindElements(By.Id(itsLinkedControlId)).FirstOrDefault();
				return new ControlWrapperBase(control, "Control with id '" + itsLinkedControlId + "' as referenced in For attribute of Label with text '" + LabelText+"'", Browser);
			}
		}
	}
}