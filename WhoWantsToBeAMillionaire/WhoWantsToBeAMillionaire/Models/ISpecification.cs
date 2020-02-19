﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhoWantsToBeAMillionaire.Models
{
	public interface ISpecification<T>
	{
		public Boolean Specificied(T item);
	}
}
