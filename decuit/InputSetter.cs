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

using FluentWebUITesting.Controls;

namespace gar3t.decuit
{
	public class TextBoxSetter : IInputSetter
	{
		public bool IsMatch(ControlWrapperBase control)
		{
			return control.Element != null && control.ToTextBoxWrapper().Element != null;
		}

		public WaitWrapper SetTo(ControlWrapperBase control, string value)
		{
			var textbox = control.ToTextBoxWrapper();
			textbox.Text().SetValueTo(value);
			return new WaitWrapper();
		}
	}

	public class CheckBoxSetter : IInputSetter
	{
		public bool IsMatch(ControlWrapperBase control)
		{
			return control.Element != null && control.ToCheckBoxWrapper().Element != null;
		}

		public WaitWrapper SetTo(ControlWrapperBase control, string value)
		{
			var checkbox = control.ToCheckBoxWrapper();
			return checkbox.CheckedState().SetValueTo(CheckedState.GetFor(value).Value);
		}
	}

	public interface IInputSetter
	{
		bool IsMatch(ControlWrapperBase control);
		WaitWrapper SetTo(ControlWrapperBase control, string value);
	}

	public class DropDownListSetter : IInputSetter
	{
		public bool IsMatch(ControlWrapperBase control)
		{
			return control.Element != null && control.ToDropDownListWrapper().Element != null;
		}

		public WaitWrapper SetTo(ControlWrapperBase control, string value)
		{
			var dropDown = control.ToDropDownListWrapper();
			var option = dropDown.OptionWithText(value);
			if (option.Exists().IsTrue)
			{
				return option.Select();
			}
			option = dropDown.OptionWithValue(value);
			if (option.Exists().IsTrue)
			{
				return option.Select();
			}
			throw new AssertionException(String.Format("{0} does not have option '{1}'", control.HowFound, value));
		}
	}
}