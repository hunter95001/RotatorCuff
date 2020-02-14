/****************************************
FILE NAME: som cluster<br>
VERSION: Ver_1.0  <br>
PROGRAMER:공영재<br>
PROJECT: som cluser<br>
GROUP: https://hunter95001.github.io/crystalfox.github.io/<br>
CLASS: <br>
        AI<br>
        -2 Layer Fuzzy SOM<br>
<br>
        Pretreatment<br>
   Step#1 ROI<br>
   Step#2 Fuzzy Stretching <br>
   Step#3 Binarization<br>
   Step#4 Morphology<br>
   Step#5 Edge<br>
   Step#6 Labelling<br>
   Step#7 Quick hull<br>
<br>
EXPLANATION:     <br>
     1-32번 까지 부분 사진<br>
     33-72번 까지 전체 사진<br>
     사진의 명암도 그래프와<br>
     의사 사진의 명암도 영역의 그래프가 흡사하다<br>
<br>
     비지도 학습 - 2Layer Fuzzy Som<br>
<br>
     33~72번 까지 사진의 명암도를 분석해<br>
     ART 클러스터링을 이용해서 비슷한 명암도를 군집화 시켜주고 학습한다<br>
     입력된 사진의 명암도를 분석해 의사결정 트리를 이용해 학습된 군집화 중 선택<br>
     선택된 이미지를  Fuzzy Som 기법을 이용해서 영역을 추출합니다<br>
****************************************/<br>
