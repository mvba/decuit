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

using System.Collections.Generic;

namespace gar3t.decuit
{
	public class CheckedState
	{
		private static Dictionary<string,CheckedState> _checkedStates = new Dictionary<string, CheckedState>();
		public static readonly CheckedState Checked = new CheckedState(true);
		public static readonly CheckedState Unchecked = new CheckedState(false);

		private CheckedState(bool value)
		{
			_checkedStates.Add(value.ToString(), this);
			Value = value;
		}

		public bool Value { get; private set; }

		public static CheckedState GetFor(string key)
		{
			return _checkedStates[key];
		}
	}
}