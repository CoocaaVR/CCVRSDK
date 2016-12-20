# CoocaaVR SDK

### 更新日志

---

###### Version:1.1.2 Date:2016.12.20
1. 新增音量按键回调；2.修复不识别U3d设置的应用名和包名

###### Version:1.1.1 Date:2016.12.05
1. 替换libsvrapi.so、libsvrplugin.so，提高稳定性

###### Version:1.1.0 Date:2016.11.23
1. 修复VR启动时序错乱导致崩溃

###### Version:1.0.3.032 Date:2016.11.16
1. 解决demo boxworld运 异常的问题

###### Version:1.0.3.031 Date:2016.11.07
1. 新增AndroidManifast 件
2. 替换libsvrspi.so
3. 新增VR模式退出推迟
4. 新增补充事项

###### Version:1.0.0.031 Date:2016.10.24
1. 建档

---

### 导入和使用

1. 在你的项目中，从`Assets -> Import Package -> Custom Package`导入`CoocaaVR_SDK.unitypackage`文件。
2. 此时在Unity中出现对话框，请保留所有复选框，并选择导入。
3. 将 `SvrCamara.prefab`(路径为:`/ SVR/Prefabs/SvrCamara`)拖动到场景中。**注意:请保留原来tag为MainCamara的Camara。**
4. 创建`SvrCamera`的实例并运 ，显示两个摄像头表示配置完成。(使用本SDK，需要在VR设备上进行调试，PC上显示的一些异常信息不影响VR设备正常使)
5. 在工程里搜索文件`Androidmanifest`，将Androidmanifest 文件中的`android:label`字段的值修改为apk显示名称，`pack-age`的值改为apk包名。  

---

### 编译配置

1. 选择`File -> Build Settings -> Android`，然后选择`player setting`
2. 在`Resolution and Presentation`部分中，将`Default Orientation`默认 向设置为`Landscape Left`
3. 在`Other Settings`中，禁用`Multithreaded Rendering`
4. 选择`Edit -> Project Settings -> Quality`
	* Anisotropic Textures = Per Texture
	* Anti Aliasing = Disabled
	
可以在场景中，实例化的SvrCamera节点的属性中启用`Anti Aliasing`。 注意，必须在SvrCamera上禁用HDR才能正确创建`anti-aliased eye buffers`:

* Anisotropic Textures = Per Texture

---

### 其他事项
1. 场景跳转前和退出应用前，请调用下面代码释放VR Camera

		SvrPlugin.Instance.EndVr ();
2. 触摸板事件转成方向按键可参考`SVR/Coocaa/CCJoystick.cs`代码
3. 返回按钮可以监听`KeyCode.Escape`
4. 暂未开放Native OpenGL ES应用程序集成


**END**
