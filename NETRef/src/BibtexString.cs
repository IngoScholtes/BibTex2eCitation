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
namespace net.sf.jabref
{

    public class BibtexString
    {

        string _name, _content, _id;

        public BibtexString(string id, string name, string content)
        {
            _id = id;
            _name = name;
            _content = content;
        }

        public string getId()
        {
            return _id;
        }

        public void setId(string id)
        {
            _id = id;
        }

        public string getName()
        {
            return _name;
        }

        public void setName(string name)
        {
            _name = name;
        }

        public string getContent()
        {
            return ((_content == null) ? "" : _content);
        }

        public void setContent(string content)
        {
            _content = content;
        }

        public object clone()
        {
            return new BibtexString(_id, _name, _content);
        }

    }
}