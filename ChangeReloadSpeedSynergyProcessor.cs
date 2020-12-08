using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;

namespace NerfedWitchPistol
{
    class ChangeReloadSpeedSynergyProcessor : MonoBehaviour
	{
		public void Awake()
		{
			this.m_gun = base.GetComponent<Gun>();
		}

		public void Update()
		{
			bool flag = this.m_gun && this.m_gun.OwnerHasSynergy(this.RequiredSynergy);
			if (flag && !this.m_processed)
			{
				this.m_processed = true;
				this.m_cachedReloadTime = this.m_gun.reloadTime;
				this.m_gun.reloadTime = 0f;
			}
			else if (!flag && this.m_processed)
			{
				this.m_processed = false;
				this.m_gun.reloadTime = this.m_cachedReloadTime;
			}
		}

		public float SynergyReloadTime;
		public string RequiredSynergy;
		private bool m_processed;
		private Gun m_gun;
		private float m_cachedReloadTime;
	}
}
