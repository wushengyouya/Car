using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class PictureHelper
    {
        /// <summary>
        /// 根据商品图片名字删除本地图片
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool DeltePicture(string path,List<string> list)
        {
            string pic = null;
            try
            {
                foreach (var item in list)
                {
                    pic = item;
                    var  yearDirectory = item.Substring(0,item.LastIndexOf("/", StringComparison.Ordinal));//年目录
                    File.Delete(path + Setting.GoodsPicFolder + item);

                    //如果改文件夹下没有文件就删除该文件夹
                    if (Directory.GetFiles(path+Setting.GoodsPicFolder+yearDirectory,".",SearchOption.AllDirectories).LongLength==0)
                    {
                        Directory.Delete(path + Setting.GoodsPicFolder + yearDirectory);//删除目录下的空文件夹
                    }
                   
                    
                }
            }
            catch (Exception e)
            {
                LogHelper.Error("logerror", "删除商品图片异常,图片名:" + pic, e);
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 删除用户本地的图片
        /// </summary>
        /// <param name="name">用户头像的名字</param>
        /// <returns></returns>
        public static bool DeltePicture(string path,string name)
        {
            try
            {
                File.Delete(path+Setting.HeadPicFolder + name);
            }
            catch (Exception e)
            {
                LogHelper.Error("logerror", "删除用户头像异常,头像名:" + name, e);
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}
