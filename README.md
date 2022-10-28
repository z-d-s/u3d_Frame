# u3d_Frame u3d框架结构

Unity项目一级目录结构
  -AssetsPackages:所有的游戏资源都存放在这个目录下
      --GUI:存放每个UI界面的GUI图片等相关
      --Sounds:存放游戏的音乐与音效
      --Maps:存放游戏的地图场景，可以在里面再继续
  -Editor:框架与项目编辑器扩展代码
  -Scenes:所有的游戏场景，包括运行场景，地图编辑场景，角色编辑场景，特效编辑场景
  -Scripts:所有游戏的代码，含框架与游戏逻辑代码
  -3rd:第三方插件存放的位置
  -StreammingAssets:用来存放打包以后的ab资源包，这样ab包能打入安装包
