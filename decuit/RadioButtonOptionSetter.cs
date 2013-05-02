using FluentWebUITesting.Controls;

namespace gar3t.decuit
{
	public class RadioButtonOptionSetter : IInputSetter
	{
		public bool IsMatch(ControlWrapperBase control)
		{
			return control.Element != null && control.ToRadioButtonOptionWrapper().Element != null;
		}

		public WaitWrapper SetTo(ControlWrapperBase control, string value)
		{
			var radioButton = control.ToRadioButtonOptionWrapper();
			return radioButton.Select();
		}
	}
}