using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TQVaultData;

namespace TQEditorAE.Events
{
	public class CharacterSelectedEvent : PubSubEvent<PlayerInfo> 
	{
	}
}
