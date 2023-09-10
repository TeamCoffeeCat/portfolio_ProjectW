using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ProjectW.Editor
{
    /// <summary>
    /// ������Ʈ �� ���� �������� ������ ��������� �����Ͽ�
    /// �ݹ��� �����Ű�� Ŭ����
    /// </summary>
    public class ProjectWAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            StaticDataImporter.Import(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
        }
    }
}