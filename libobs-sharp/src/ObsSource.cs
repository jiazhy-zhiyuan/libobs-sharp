﻿/***************************************************************************
	Copyright (C) 2014 by Ari Vuollet <ari.vuollet@kapsi.fi>
	
	This program is free software; you can redistribute it and/or
	modify it under the terms of the GNU General Public License
	as published by the Free Software Foundation; either version 2
	of the License, or (at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, see <http://www.gnu.org/licenses/>.
***************************************************************************/

using System;

namespace OBS
{
	public class ObsSource
	{
		internal unsafe libobs.obs_source* instance;    //pointer to unmanaged object

		public unsafe ObsSource(ObsSourceType type, string id, string name/*, obs_data settings*/)
		{
			instance = (libobs.obs_source*)libobs.obs_source_create((libobs.obs_source_type)type, id, name, IntPtr.Zero);

			if (instance == null)
				throw new ApplicationException("obs_source_create failed");

			libobs.obs_source_addref((IntPtr)instance);
		}

		~ObsSource()
		{
			Release();
		}

		public unsafe void Release()
		{
			if (instance == null)
				return;

			libobs.obs_source_release((IntPtr)instance);
			instance = null;
		}

		public unsafe ObsSource(libobs.obs_source* instance)
		{
			this.instance = instance;
			libobs.obs_source_addref((IntPtr)instance);
		}

		public unsafe libobs.obs_source* GetPointer()
		{
			return instance;
		}


		public unsafe String Name
		{
			get
			{
				return libobs.obs_source_get_name((IntPtr)instance);
			}
		}

		public unsafe uint Width
		{
			get
			{
				return libobs.obs_source_get_width((IntPtr)instance);
			}
		}

		public unsafe uint Height
		{
			get
			{
				return libobs.obs_source_get_height((IntPtr)instance);
			}
		}


        public unsafe ObsData GetSettings()
        {
            IntPtr ptr = libobs.obs_source_get_settings((IntPtr)instance);

            if (ptr == IntPtr.Zero)
                return null;

            return new ObsData(ptr);
        }

		public unsafe void AddFilter(ObsSource filter)
		{
			libobs.obs_source_filter_add((IntPtr)instance, (IntPtr)filter.GetPointer());
		}

		public unsafe void Render()
		{
			libobs.obs_source_video_render((IntPtr)instance);
		}
	}

	public enum ObsSourceType : int
	{
		Input,
		Filter,
		Transition,
	};
}