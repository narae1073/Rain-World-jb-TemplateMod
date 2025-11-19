using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace My_Nice_Rain_World_Mod.Utils
{
    public static class ElementParser
    {
        /// <summary>
        /// 엔터로 구분된 멀티 라인 문자열을 개별 엘리먼트 문자열 배열로 분리합니다.
        /// </summary>
        /// <param name="rawElementsString">파일에서 읽어온 전체 멀티 라인 문자열입니다.</param>
        /// <returns>개별 엘리먼트 문자열을 담은 배열 (string[])</returns>
        public static string[] LoadElementsFromFile(string filePath)
        {
            // 1. 파일 존재 여부 확인
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[ERROR] 파일을 찾을 수 없습니다: {filePath}");
                // 파일이 없으면 빈 배열을 반환하여 프로그램이 멈추는 것을 방지합니다.
                return Array.Empty<string>();
            }

            // 2. 파일의 모든 내용을 문자열로 읽어옵니다.
            string rawElementsString = File.ReadAllText(filePath);

            // 3. 읽어온 문자열을 분리, 정리하여 배열로 반환합니다. (LINQ 사용)
            return rawElementsString
                // OS 환경에 맞는 줄 바꿈 문자로 분리하고, 빈 줄은 제거합니다.
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)

                // 각 줄의 앞뒤 공백을 제거합니다.
                .Select(line => line.Trim())

                // 공백만 있던 줄이 Trim()으로 인해 완전히 비어버린 경우를 제거합니다.
                .Where(line => !string.IsNullOrEmpty(line))

                // 최종적으로 string[] 배열로 변환합니다.
                .ToArray();
        }
    }
}
