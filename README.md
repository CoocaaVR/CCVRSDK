# # CoocaaVR SDK

### 更新日志

---

###### version:1.1.1 date:2016.11.23
1. 替换几个so库

###### version:1.1.0 date:2016.11.23
1. 修复VR启动时序错乱导致崩溃

###### version:1.0.3.032 date:2016.11.16
1. 解决demo boxworld运 异常的问题

###### version:1.0.3.031 date:2016.11.07
1. 新增AndroidManifast 件
2. 替换libsvrspi.so
3. 新增VR模式退出推迟
4. 新增补充事项

###### version:1.0.0.031 date:2016.10.24
1. 建档

---

### 导入和使用

1. 在你的项目中，从`Assets -> Import Package -> Custom Package`导入`CoocaaVR_SDK.unitypackage`文件。
2. 此时在Unity中出现对话框，请保留所有复选框，并选择导入。
3. 将 `SvrCamara.prefab`(路径为:`/ SVR/Prefabs/SvrCamara`)拖动到场景中。**注意:请保留原来tag为MainCamara的Camara。**
4. 创建`SvrCamera`的实例并运 ，显示两个摄像头表示配置完成。(使用本SDK，需要在VR设备上进行调试，PC上显示的一些异常信息不影响VR设备正常使)
5. 在工程里搜索文件`Androidmanifest`，将Androidmanifest 文件中的`android:label`字段的值修改为apk显示名称，`pack-age`的值改为apk包名。  
