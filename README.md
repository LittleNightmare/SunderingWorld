# SunderingWorld

国服卫月[已经集成](https://github.com/ottercorp/Dalamud/commit/b9c193d13c1f65c9675b92798a83848519b05998)，本插件停止更新

**CN only** plugin would casue problem on other server. DON't use it.

本插件效仿海德林重新划分世界，让服务器真的成为了服务器

人话版：插件修改`World`和`WorldDCGroupType`，将国际服的服务器的`IsPublic`设置为`False`，修改了国服服务器相关并添加对应四个大区。

总体来说，提升对于使用这两个表格数据插件的兼容性。一般来说，如果国际服插件内部有手动可以填写服务器的地方，就会用到。比方说`Visibility`

请注意，对于修改的数据，卸载插件**无法清除**，需要重启游戏

## TODO

[] 卸载插件时，恢复数据

## Thanks

感谢[AsterOcclu](https://github.com/AsterOcclu)的Utils4CN。这个插件就是把Utils4CN单独拿出来了
