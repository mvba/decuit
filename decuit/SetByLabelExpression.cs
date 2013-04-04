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

using FluentAssert;

using FluentWebUITesting.Controls;
using FluentWebUITesting.Extensions;

using OpenQA.Selenium;

namespace gar3t.decuit
{
	public static class SetterFor
	{
		public static SetByLabelExpression ControlWithLabel(IWebDriver browser, string labelText)
		{
			return new SetByLabelExpression(browser, labelText);
		}

		public static SetByIdExpression ControlWithId(IWebDriver browser, string id)
		{
			return new SetByIdExpression(browser, id);
		}
	}

	public abstract class SetExpression
	{
		protected IWebDriver Browser { get; private set; }
		private static readonly List<IInputSetter> _setters = new List<IInputSetter>
		{
			new TextBoxSetter(),
			new CheckBoxSetter(),
			new DropDownListSetter()
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

	public class SetByIdExpression : SetExpression
	{
		public string Id { get; private set; }

		public SetByIdExpression(IWebDriver browser, string id)
			: base(browser)
		{
			Id = id;
		}

		public override ControlWrapperBase Control
		{
			get
			{
				var control = Browser.FindElements(By.Id(Id)).FirstOrDefault();
				return new ControlWrapperBase(control, String.Format("Control with id '{0}'", Id), Browser);
			}
		}
	}

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