
これは何？
・指定した時間の間、頭部を動かさなければ、視界を覆う黒い球体が出現します。
　HMDを装着したまま寝落ちしたときの目へのダメージを軽減する目的に作成しました。

仕組み
・4フレーム前の座標を保存し、現在のオブジェクトの座標と比較し移動量が ResetMoveDistance の値より小さければタイマーのカウントを進め、移動量が大きければカウントをリセットします。
・タイマーが WaitTime(Minutes) の値より大きくなれば黒い Sphere を描画し、視界を覆います。
・カメラを使用しておりローカルでのみ動作するため、他人から黒い球体が見えることはありません。
・カメラや鏡には描画されないように設定済です。

導入時の注意点
・Sleep_3.0.prefab 及び内部のオブジェクトの Transfrom の Scale の値は決して変えないでください。動作しなくなります。

導入方法(Avatars3.0用)
1.Sleep_3.0.prefab をアバター直下に置く。
2.Sleep_3.0.prefab の座標はアバター頭部の中に移動する。
3.SleepSphere のマテリアルにおいて WaitTime(Minutes) を0にすると黒い球体が見えることを確認し、マテリアルの Scale を操作して頭を覆う程度に大きくする。
4.SleepSphere の SkinnedMeshRenderer において、 Bounds-Extent の値を調整しアバター頭部を覆う大きさにする。
5.マテリアルの WaitTime(Minutes) の値を設定する（30分なら30に）。
6.Sleep_3.0.prefab の ParentConstraint の Sources にアバターの Head ボーンを設定する。
7.アバターをアップロードして導入終わり。

メモ
・ワールド側の移動速度の設定により移動してもタイマーがリセットされないこともあるので、一番簡単なリセット方法はVRCのMenuから Standing か Sitting を切り替えることです。これにより頭部が瞬間的に大きく動くのでリセットされやすいです。
・VRCのMenuは黒い球体より優先されて描画されるので視界が完全に遮られることはないはずです。

履歴
2021/04/09 公開
2021/04/26 他カメラにmemoryが描画する縦線が映り込んでしまうのを修正
2023/06/16 現環境に合わせた修正
Released under the MIT license

製作者
わた
VRChat：wata_pj Twitter：@wata_pj
不具合報告はこちらへご連絡ください。