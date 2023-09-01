# u3d_Frame u3d框架结构

Unity项目一级目录结构：

	-3rd:第三方插件存放的位置
	
  -AssetsPackage:所有的游戏资源都存放在这个目录下，资源如何分类又决定了打ab包时候的资源管理等，所以尽可能的把资源按照功能分类放好
      --GUI:存放每个UI界面的GUI图片等相关
      --Sounds:存放游戏的音乐与音效
      --Maps:存放游戏的地图场景，可以在里面再继续
      --Charactors:存放角色动画等相关资源
      --Effects:存放粒子特效等相关资源
      --Excels:存放游戏中配置文件的表格数据
      
  -Editor:框架与项目编辑器扩展代码
  		--AssetBundle:编辑器扩展ab包自动化打包管理，版本管理等编辑器相关代码
  		--Common:一些编辑器扩展的公用代码
  		--ExcelBuild:表格处理工具代码
  		--GameTools:
  		--GUIBuild:
  		--PackageBuild:打包时候的工具代码
  
  -Scenes:所有的游戏场景，包括运行场景，地图编辑场景，角色编辑场景，特效编辑场景
  
  -Scripts:所有游戏的代码，含框架与游戏逻辑代码
  		--3rd:用来存放纯粹的第三方C#代码，不是插件，比如protobuf for C#等
  		--Game:用来存放游戏逻辑代码
  		--Utils:用来存放游戏工具类的代码，比如单例，MD5，SHA1
  		--Managers:用来存放主要的管理模块代码（资源管理，网络模块，声音模块等）
  		--AssetsBundle:与ab资源包管理更新相关的代码
  
  -StreammingAssets:用来存放打包以后的ab资源包，这样ab包能打入安装包

与Assets同级会有一个AssetBundles文件夹，存放打出的AssetsBundle包
		每个平台，每个渠道的ab包是不一样的，所以AssetBundles目录下，先分平台，再分渠道
		-Android
				--xxx平台
		-iOS
				--xxx平台