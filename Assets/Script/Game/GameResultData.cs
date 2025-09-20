using UnityEngine;


    // 어디든 Scripts 폴더 안에 새 C# 스크립트로 만들기
    public static class GameResultData
    {
        // 최종 결과 (씬 바뀌어도 유지됨)
        public static int[] finalLevels;
        public static int[] maxLevels;

        // 재시작/다시하기 등을 대비해 초기화 함수도 준비
        public static void Clear()
        {
            finalLevels = null;
            maxLevels = null;
        }
    }


