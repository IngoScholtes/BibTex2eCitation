/*  Copyright (C) 2003-2011 JabRef contributors.
    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
*/
using System;
using System.Collections.Generic;

namespace net.sf.jabref {
    
/**
 * This class represents a list of comparators. The first IComparable takes precedence,
 * and each time a IComparable returns 0, the next one is attempted. If all comparators
 * return 0 the readonly result will be 0.
 */
public class FieldComparatorStack<T> : IComparer<T> {

    List<IComparer<T>> comparators;

    public FieldComparatorStack(List<IComparer<T>> comparators)
    {
        this.comparators = comparators;
    }

    public int Compare(T o1, T o2) {
    	foreach (var comp in comparators){
            int res = comp.Compare(o1, o2);
            if (res != 0)
                return res;
        }
        return 0;
    }
}
}
