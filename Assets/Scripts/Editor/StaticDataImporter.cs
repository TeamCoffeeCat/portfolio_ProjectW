using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProjectW.Editor
{
    /// <summary>
    /// ���� ������ �߰��Ǿ��� �� ��ó���� ����
    ///  -> excel ������ �߰�/������ �����ϰ�, json���� ��ȯ
    /// </summary>
    public class StaticDataImporter
    {
        /// <summary>
        /// ���� ��ó���⿡�� ���� ���氨�� �ݹ��� ����� ��, ȣ���� �޼���
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        public static void Import(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            ImportNewOrModified(importedAssets);
            Delete(deletedAssets);
            Move(movedAssets, movedFromAssetPaths);
        }

        /// <summary>
        /// ������ ������ ��� ������ ���
        /// </summary>
        /// <param name="deletedAssets">������ ���� ����</param>
        private static void Delete(string[] deletedAssets)
        {
            ExcelToJson(deletedAssets, true);
        }

        /// <summary>
        /// ������ �̵� ���� ��
        /// </summary>
        /// <param name="movedAssets">���ο� ���(�̵� ��)�� ���� ����</param>
        /// <param name="movedFromAssetPaths">���� ���(�̵� ��)�� ���� ����</param>
        private static void Move(string[] movedAssets, string[] movedFromAssetPaths)
        {
            // ���� ��� ���� ���� ��� ����
            Delete(movedFromAssetPaths);
            // ���ο� ��� ���� ����
            ImportNewOrModified(movedAssets);
        }

        /// <summary>
        /// ������ ���� �ҷ����ų� �������� ��
        /// </summary>
        /// <param name="importedAssets">�ҷ����ų� ������ ���� ����</param>
        private static void ImportNewOrModified(string[] importedAssets)
        {
            ExcelToJson(importedAssets, false);
        }

        /// <summary>
        /// ���� -> Json ��ȯ ���
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="isDeleted"></param>
        private static void ExcelToJson(string[] assets, bool isDeleted)
        {
            // �Ķ���ͷ� ���޹��� assets �迭�� ������ ��
            // ���� ������ ������ ��θ� ���� ����Ʈ
            List<string> staticDataAssets = new List<string>();

            // ���� ��ο��� �������ϸ� �ɷ�����
            foreach (var asset in assets)
            {
                if (IsStaticData(asset, isDeleted))
                    staticDataAssets.Add(asset);
            }

            // �ɷ��� excel ��ȹ�����͸� json���� ��ȯ
            foreach (var staticDataAsset in staticDataAssets)
            {
                try
                {
                    var rootPath = Application.dataPath;
                    // ���ڿ����� ������ / �� �����ϴ� �κк��� ���� ���ڿ��� ���� ����
                    // ��������� Assets ���� ��ΰ� ������
                    rootPath = rootPath.Remove(rootPath.LastIndexOf('/'));

                    // �����θ� ����
                    var absolutePath = $"{rootPath}/{staticDataAsset}";

                    var converter = new ExcelToJsonConvert(absolutePath, $"{rootPath}/{Define.StaticData.SDJsonPath}");

                    // ��ȯ ���� �� ����� ��ȯ�޾� �����ߴ��� Ȯ��
                    if (converter.SaveJsonFiles() > 0)
                    {
                        // ��ο��� �����̸��� Ȯ���ڸ� ����.
                        var fileName = staticDataAsset.Substring(staticDataAsset.LastIndexOf('/') + 1);
                        // Ȯ���ڸ� �����ؼ� �����̸��� �����.
                        fileName = fileName.Remove(fileName.LastIndexOf('.'));

                        // json ������ �����Ͽ� ������Ʈ ���� ���� ��ġ������ ��
                        // ������ �󿡼� �ε��Ͽ� �νĽ�Ű�� �۾��� ���� �ʾ����Ƿ�
                        // �����Ϳ��� �ν��� �� �ֵ��� ����Ʈ �Ѵ�.
                        AssetDatabase.ImportAsset($"{Define.StaticData.SDJsonPath}/{fileName}.json");
                        Debug.Log($"#### StaticData {fileName} reimported");
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogErrorFormat("Couldn't convert assets = {0}", staticDataAsset);
                    EditorUtility.DisplayDialog("Error Convert",
                        string.Format("Couldn't convert assets = {0}", staticDataAsset), "OK");
                }


            }
        }

        /// <summary>
        /// ������ ���� �����̸鼭 ��ȹ���������� üũ
        /// </summary>
        /// <param name="path">�ش� ���� ���</param>
        /// <param name="isDeleted">���� �̺�Ʈ����?</param>
        /// <returns></returns>
        private static bool IsStaticData(string path, bool isDeleted)
        {
            // xlsx Ȯ���ڰ� �ƴ϶�� ����
            if (path.EndsWith(".xlsx") == false)
                return false;

            // ���� ���翩�� Ȯ���� ����, ������ ��ü��θ� ����
            //  -> �����ͻ󿡼� dataPath : ����̺���� ������Ʈ �������� ��α���
            //     �Ķ���ͷ� ���޹��� ��δ� ������������
            var absolutePath = Application.dataPath + path.Remove(0, "Assets".Length);

            // Assets/StaticData/Excel ������ �����ϴ� ���������� ��ȹ������ ��� ��Ģ�� ������
            // ���� �ش� ��ο� �������� �ʴ´ٸ� ��ȹ�����Ͱ� �ƴ�

            return (isDeleted || File.Exists(absolutePath)) && path.StartsWith(Define.StaticData.SDExcelPath);
        }
    }
}