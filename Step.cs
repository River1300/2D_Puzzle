/*
1. 물리 퍼즐 게임 - 물리 기반 퍼즐 밑바탕 만들기

    #1. 모바일 환경 설정
        [a]. 캐쥬얼 물리 기반 게임을 만들기 위해 안드로이드를 위한 세팅을 진행한다.
        [b]. 빌드 세팅에서 안드로이드로 스위치 플랫폼을 진행한다.
    
    #2. 에셋 다운 받기

    #3. 배경 만들기
        [a]. 메인 카메라 설정
            배경 색 지정
            프로젝션 : 오쏘그래픽
            사이즈 9
        [b]. 배경 스프라이트 하이어라키 창에 등록
        [c]. 해상도 + : 비율을 9 : 19로 지정
        [d]. 2D 스프라이트 스퀘어 생성
            오더인 레이어 백그라운드를 -1로
            스퀘어 사이즈 10 5 1
            스퉤어 위치 0 -11 0
            바닥이므로 색을 어둡게 지정
            물리적인 효과를 위해 리지드바디2D
                바디 타입은 움직이지 않으므로 스태틱
            박스 콜라이더2D
        [e]. 바닥을 복사하여 우측 좌측 벽으로 활용
            사이즈 1 20 1
            위치 4.7
        [f]. 경계와 배경을 모두 빈 오브젝트에 모은다.
        [g]. 테스트를 위해 2D 스프라이트 서클을 만들고 리지드 바디 부착
            서클 콜라이더도 부착
            떨어 트려 본다.

    #4. 동글이 기본 로직
        [a]. 스크립트 생성 및 부착
        [b]. 마우스 움직임에 따라 동글이가 좌우로 움직이다가 마우스 클릭을 땐 순간 자유낙하 하도록 한다.
            (마우스의 입력, 리지드 바디)
        [c]. 마우스의 위치를 받아서 저장한다. mousePosition
            스크린 좌표계를 월드 좌표계로 바꾸기 위해 카메라 함수를 활용한다.
        [d]. 동글이의 위치를 마우스 위치에 배정한다.
        [e]. 동글이의 리지드 바디에서 시뮬레이트 기능을 정지하여 물리적인 계산을 멈추도록 한다.
        [f]. 카메라의 z축이 -10인 상태인데 카메라를 통해 마우스의 위치 값을 받아서 동글이의 위치에도 -10의 값이 적용된다.
            저장을 해둔 벡터의 z축을 고정하도록 한다.
        [g]. 벡터 러프 기능을 이용하여 목표 지점까지 천천히 이동하게 한다.
        [h]. 경계선을 배치하고 y축을 고정하여 경계선 위로만 이동을 한정한다.
            경계선에 콜라이더를 부착한다.
        [i]. 경계선을 넘지 못하도록 로직으로 이동 제한을 건다.
            좌우 이동 제한을 할 값을 저장한다. 사이드 경계선 위치값과 동글이의 반지름을 더한 값
            만약에 마우스의 x좌표 위치를 좌우 경계선 값과 비교하여 위치 고정

    #5. 끌어다 놓기
        [a]. 눌럿다, 땟다를 구분하기 위해 bool 변수, 물리 효과를 위한 리지드 바디를 속성으로 갖는다.
        [b]. 리지드 바디를 초기화 한다.
        [c]. Drag 함수를 만든다.
            플래그에 true
        [d]. Drop 함수를 만든다.
            플래그에 false
            꺼 두었던 시뮬레이터를 활성화
        [e]. 저장한 플래그를 활용하여 Update() 함수에서 제어문으로 활용하여 이동 로직을 실행한다.
        [f]. 이미지를 만들어서 버튼으로 활용한다.
            캔버스 크기를 가득 채운뒤 알파 값을 0으로 맞춘다.
        [g]. 이미지에 이벤트 트리거 컴포넌트를 부착
            포인터 다운, 포인터 업으로 Drag, Drop 함수를 등록
        [h]. 동글이에게 태그를 달아 준다.
        [i]. 완성된 동글이를 에셋으로 저장한다.
*/

/*
2. 물리 퍼즐 게임 - 프리펩으로 다양한 동글이 생성하기

    #1. 동글이를 관리하는 게임 매니저 만들기
        [a]. 빈 오브젝트를 만들고 게임 매니저 명명
            스크립트 생성 및 부착
        [b]. 동글이를 저장해둘 스크립트 타입의 변수가 필요
            동글이를 넘겨 준다.
        [c]. 게임 매니저가 동글이 대신에 Drag, Drop를 호출할 것이다.
        [d]. TouchDown(), TouchUp() 함수를 만들어서 동글이의 함수를 대리 호출한다.
            이벤트 트리거 함수 게임 매니저의 함수로 바꿔준다.

    #2. 게임 매니저가 동글이를 만든다.
        [a]. 동글이를 부르는 함수를 만든다.
            생성 함수를 호출한다.
            반환된 동글이를 동글이 변수로 받는다.
            미리 만들어 두었던 동글이 속성에 새로 만든 동글이를 만든다.
        [b]. 동글이를 생성하는 함수를 만든다.
            프리팹을 이용하여 인스턴트를 만든다.
            인스턴트를 만들 때 위치 값을 전달한다.
            반환값으로 동글이 스크립트를 주도록 한다.
            인스턴스를 받기 위한 게임 오브젝트로 받고 그 인스턴스로 스크립트를 받아서 저장한다.
            저장한 스크립트를 반환한다.
        [c]. 프리팹을 담을 변수가 필요하다.
        [d]. 동글이의 초기 위치를 담을 빈 오브젝트를 만든다.
            이 빈 오브젝트에 만들어지는 인스턴스들이 담긴다.
            좌표 y 8
        [e]. 위치 값으로 빈 오브젝트를 변수로 받는다.
        [f]. 테스트를 위해 Start() 함수에서 동글이를 부르는 함수를 호출하도록 한다.
            프리팹의 자체 좌표가 초기화 되었는지 확인한다.
        [g]. 게임이 시작하면서 동글이가 나오지도 않았는데 마우스를 클릭하면 에러가 발생한다.
            TouchDown()에서 동글이 속성이 비어 있다면 반환한다.
            TouchUp()도 마찬가지다. 추가로 동글이를 정상적으로 추락 시켰다면 동글이 속성을 null로 비워 준다.
    
    #3. 계속해서 동글이를 생성한다.
        [a]. 동글이를 부르는 함수에서 재귀로 동글이를 계속 만들어 보자.
            코루틴 함수를 호출한다.
        [b]. 코루틴 함수로 동글이 호출 함수를 만든다.
            2.5초를 일단 쉬고 동글이를 부르는 함수를 호출한다.
            2.5초 쉬기 전에 동글이 속성이 null 이 아니라면 매 프레임마다 반복문을 돈다.

    #4. 다양한 레벨의 동글이를 생성한다.
        [a]. 동글이가 레벨이라는 개념을 위해 정수형 변수를 속성으로 받는다. 0 ~ 6 레벨
        [b]. 동글이 프리팹 모드로 들어가서 애니매이터를 부착한다.
            애니매이터 컨트롤러를 담는다.
        [c]. 애니매이션 클립을 만들어서 로직에서 받는 파라미터에 따라 각기 다른 레벨을 출력한다.
            Level 0
            애드 프로퍼티로 트랜스폼 스케일을 추가
            시작은 0으로 0.2초 뒤에 1로 크기 변화
            스프라이트를 기본 설클 스프라이트로 만들고 동글이 이미지를 애니매이션에 옮겨 놓는다.
            Level 1
            사이즈 1.5
            스프라이트 변경
        [d]. 애니매이터 컴포넌트를 변수로 받는다.
        [e]. 스크립트가 활성화 될 때 실행되는 이벤트 함수로 애니매이터를 실행한다. OnEnable
            애니매이터에 파라미터 값을 전달한다.
        [f]. 애니매이션에서 스케일은 0부터 시작한다. 그러므로 프리팹의 사이즈도 0으로 맞춘다.
        [g]. 동글이 스크립트를 받아온 뒤에 동글이 레벨에 접근한다.
            랜덤 클래스로 0 ~ 8사이의 값을 랜덤하게 받아서 레벨에 배정한다.
            이후 애니매이션이 실행되도록 동글이 게임 오브젝트에 접근하여 활성화 한다.
                프리팹은 비활성화 해둔다.

    #5. 프레임 설정
        [a]. Game -> State에서 프레임을 볼 수 있다.
        [b]. 게임 매니저에서 프레임을 안정적으로 잡는다.
            Application 클래스에서 targetFrameRate로 프레임을 맞춘다.
        [c]. 동글이 프리팹에서 리지드 바디를 인터폴레이트로 설정하여 물리적인 움직임을 좀더 부드럽게 만든다.
*/

/*
3. 물리 퍼즐 게임 - 물리 이벤트로 동글이 합치기

    #1. 동글이 합치기
        [a]. 동글이에게 충돌 이벤트 함수를 만들어 준다.
            Stay로 만든다.
            충돌한 오브젝트의 태그가 동글이 일 때만 그 동글이의 스크립트를 받아 온다.
            나의 레벨과 상대의 레벨이 같다면 동글이 합치기를 할 예정이다.
                그런데 이때 같은 레벨의 동글이가 여러개 뭉쳐 있는 경우가 있을 수 있다.
                    그러면 둘이 합체될 때 다른 동글이가 합치려고 하고 이러면 에러가 발생할 수 있다.
                그러므로 현재 합쳐지고 있다는 플래그를 속성으로 만든다.
            나의 레벨과 상대의 레벨이 같고 내가 합쳐지는 중이 아니고 상대도 합쳐지는 중이 아니고 레벨이 7보다 작을 때
                나와 상대편의 위치를 가져온다.
                    나의 x, y축을 변수로 저장하고 상대의 x, y축을 변수로 저장한다.
            내가 아래에 있을 때?
            동일한 높이일 때, 내가 오른쪽에 있을 때?
                내가 레벨업을 할 경우?
                    내가 레벨업 하는 함수를 호출
                        레벨업 함수를 만든다.
                            합체중 플래그에 true
                            합쳐질 때 이동을 하지 않도록 리지드바디를 통해 속도를 zero
                                회전 속도도 0
                            코루틴 레벨업 함수를 만든다.
                            코루틴 함수 호출
                                0.2초 = 상대가 나에게 합쳐지는 시간 정도 쉬었다가
                                애니매이터 파라미터로 레벨에 자신의 레벨에 +1 하여 전달(++level로 전달할 경우 주변에 있는 한 단계 높은 레벨의 동글이와 먼저 합쳐질 수 있음)
                                그리고 애니매이션이 실행되는 시간 0.3초 정도 쉰다.
                                실제 레벨 ++
                                합체 중 플래그 false
                    상대를 숨기는 함수를 호출
                        숨기기 함수를 만든다.
                            합체중 플래그에 true
                            리지드바디의 시뮬레이트를 비활성화
                            콜라이더를 비활성화 하기위해 속성으로 콜라이더를 받는다.
                            숨기기 함수도 매개변수로 상대의 위치 벡터를 받는다.
                        코루틴 숨기기 함수를 추가로 만든다.
                            매개변수로 상대의 위치를 벡터로 받는다.
                            상대의 위치는 합치는 함수를 호출할 때 전달하도록 한다.(Collision)
                            프레임 마다 이동을 할 예정이므로 프레임 카운트를 세는 변수를 만든다.
                            반복문으로 프레임 카운트가 20일 때까지 반복하도록 한다.
                            함수를 호출한 자신의 위치를 벡터 러프를 이용하여 자신을 목표 지점까지 0.5의 속도로 조금씩 움직인다.
                            다 합쳤다면 합치는 중 플래그 false
                            그리고 함수를 호출한 자신을 비활성화 한다.
        [b]. OnCollisionStay()함수가 실행이 되기 전에 리지드 바디로 같은 레벨이 튕겨져 나가는 상황이 발생한다.  

    #2. 물리 보정하기
        [a]. 세팅에서 물리 세팅이 가능하다.
            오토 싱크 트랜스폼으로 서로의 속도, 위치 살짝 다를 수 있는데 이를 맞추어 준다.

    #3. 최대로 성장한 동글이를 기준으로 게임 매니저에서 다음 레벨 동글이를 만든다.
        [a]. 게임 매니저에 최대 레벨을 저장할 속성을 만든다.
            동글이를 호출할 때 랜덤한 동글이 레벨을 지정했는데 이 때 한계선을 최대 레벨 변수로 지정한다.
        [b]. 성장할 때마다 최대 레벨을 증가시키도록 하기 위해 동글이에서 최대 레벨을 올려 주도록 한다.
            동글이에게 게임매니저 속성을 만들어 준다.
            게임 매니저에서 동글이를 만들 때 동글이에게 자기 자신을 전달한다.
        [c]. 다시 동글이로 돌아와서 레벨업 코루틴 함수에서 매니저의 최대 레벨을 증가 시킨다.
*/

/*
4. 물리 퍼즐 게임 - 멋진 이펙트 만들기

    #1. 파티클 꾸미기
        [a]. 파티클 시스템을 만든다.
            위치 조정
            이미션에서 이펙트 입자의 량을 조절한다. 버스트를 통해 한꺼번에 많은 양의 입자를 폭발시킨다.
            이펙트 모양은 서클로, 각도를 회전 시킨다.
            입자의 크기 라디우스를 줄인다.
            입자의 스프라이트를 교체하기 위해 텍스쳐 시트 애니매이션에서 모드를 스파이트로 바꾸어 준다.
                기본 스프라이트를 서클로 바꾼다.
            입자가 퍼저나가는 속도를 더 빠르게
            입자가 살아있는 시간을 0.5 ~ 1
            무한 반복을 막기 위해 루핑을 체크 해제 하고
            게임이 시작할 때 자동으로 실행되는 플레이 온 어웨이크도 해제한다.
            리미티드 벨로시티 오버 라이프 타임에서 드래그를 1로 지정하여 입자가 날아가는 반경을 제어한다.
            사이즈 오버 라이프 타임에서 사이즈를 점차 줄여 간다.
            칼라 오버 라이프 타임에서 색에 그라데이션을 넣어 준다.
            테일즈를 이용하여 입자에 꼬리나 리본을 단다.
                마테리얼을 기본 입자로 한다.
                사이즈 에펙트 라이프 타입을 체크
                윝스 오버 프레임을 커브로
            오더 레이어를 2로 하여 동글이보다 높은 곳으로
        [b]. 이름을 이펙트로 바꾸고 에셋으로 만든다.

    #2. 동글이가 이펙트 생성
        [a]. 동글이에게 파티클 시스템 속성을 준다.
            동글이와 마찬가지로 파티클도 동글이가 생성될 때 생성되어 속성에 초기화 된다.
        [b]. 게임 매니저가 이펙트 프리펩과 이펙트 위치 그룹을 속성으로 받는다.
        [c]. 게임 매니저 속성에 위치와 프리팹을 담는다.
        [d]. 게임 매니저에서 동글이를 인스턴스화 하기 전에 이펙트를 인스턴스화 한다.
        [e]. 인스턴스한 이펙트로 부터 파티클 시스템을 가져 온다.
        [f]. 동글이가 만들어진 뒤에 이펙트를 초기화 한다.

    #3. 이펙트 실행
        [a]. 동글이가 레벨업을 할 때 동글이가 그 위치에서 이펙트를 실행하도록 한다.
            이펙트 실행 함수를 만든다.
        [b]. 이펙트의 위치는 동글이의 위치로 맞춘다.
        [c]. 이펙트의 크기는 동글이의 크기와 맞춘다.
        [d]. 설정이 완료되었다면 이펙트를 Play()
        [e]. 이펙트 실행 함수는 레벨업으로 애니매이션이 실행될 때 호출하도록 한다.
*/

/*
5. 물리 퍼즐 게임 - 게임 오버 구현하기

    #1. 게임 매니저의 점수 관리
        [a]. 점수를 받을 속성을 만든다.
        [b]. 점수는 동글이가 올려줘야 한다.
            동글이가 합쳐지는 코루틴 함수에서 매니저의 점수 속성에 접근하여 자신의 레벨 제곱을 더한다.
                Mathf는 float을 반환하기 때문에 명시적 형변환으로 정수값을 반환하도록 한다.

    #2. 동글이가 경계선에 일정 시간 동안 닿는지 확인하는 경계선 이벤트
        [a]. 동글이가 다른 오브젝트와 트리거 중일 때
            그런데 그 트리거가 경계선일 때
            시간을 제는데 이를 위해 지정할 시간 속성을 만들어 준다.
            트리거와 닿아 있는 시간이 2초가 넘을 경우 색을 바꾸도록 한다.
        [b]. 동글이에게 색을 바꾸기 위해 스프라이트 렌더러 속성을 만들어 준다.
        [c]. 트리거에 닿아 있는 시간이 5초가 넘을 경우 게임 오버 시킬 예정이다.
        [d]. 게임 매니저에 게임 오버 함수를 만들어 준다.
            임시로 디버그 로그로 표시한다.
        [e]. 테스트를 해보면 로직대로 실행되지 않는다.
            동글이의 리지드 바디에 슬리핑 모드가 있는데, 이는 물리적 충돌을 어느때 할 것인지를 나타낸다.
                이를 네버슬립으로 하여 계속해서 충돌을 감지하도록 지정 한다.
            동글이 프리펩에 가서 리지드 바디의 슬리핑 모드를 수정하도록 한다.
        [f]. 게임 오버가 지속적으로 호출되고 있다. 이를 단 한 번으로 수정하도록 한다.
        [g]. 게임 매니저에게 현재 게임 오버 상태인지를 체크할 변수를 속성으로 만든다.
        [h]. 게임 오버 함수에서 현재 게임 오버 플래그를 확인하여 반환을 한다.
        [i]. 처음 호출될 때는 게임 오버 플래그를 true
        [j]. 만약에 동글이가 경계면에 닿아 있다가 나갔을 경우에는 시간을 초기화 해줘야 한다.
            동글이에게 트리거 탈출 함수를 만든다.
            태그를 통해 경계선에서 탈출했는지 확인하고 탈출 했다면
                시간은 0
                색깔을 다시 원상복구 시킨다.

    #3. 게임 오버 구현
        [a]. 스테이지에 쌓여있는 동글이 들을 모두 지울 예정이다.
        [b]. 씬에 활성화 되어 있는 모든 동글이 가져오기
            컴포넌트를 통해서 씬에 등록된 게임 오브젝트를 찾는 FindeObjectsOfType<>을 활용한다.
                동글 스크립트를 가지고 있는 모든 오브젝트를 동글이 스크립트 배열로 받는다.
        [c]. 반복문으로 배열에 하나씩 접근하여 동글이의 Hide() 함수를 호출한다.
            이 함수는 위치 벡터를 매개변수로 받는데 이는 화면 밖의 임의의 수를 그냥 전달하도록 한다.
        [d]. 동글이 스크립트의 Hide()함수로 가서 분기점을 제어문으로 만들어 준다.
            매개변수로 받은 값이 임의의 값이 아닐 경우에만 합치기 로직을 실행한다.
            임의의 값이라면 크기를 줄여서 사라지게 한다.
                벡터의 러프를 활용하여 0으로 만든다.
        [e]. 동글이가 시간차를 두면서 하나씩 사라지도록 수정한다.
        [f]. 코루틴 게임 오버 함수를 만든다.
        [g]. 게임 오버 함수에 만들어 두었던 동글이 찾기 제거하기 로직을 코루틴으로 옮긴다.
        [h]. 반복문 안에 딜레이 0.1초를 지정한다.
        [i]. 게임 오버 함수가 코루틴을 호출하도록 한다.
        [j]. 딜레이를 통해 하나씩 사라질 경우 사라지면서 다른 동글이가 합쳐질 수도 있다.
            게임 오버가 되는 순간 동글이들의 모든 물리 효과를 끄도록 한다.
        [k]. 동글이를 지우기 전에 다른 반복문으로 해당 동글이의 리지드 바디에 접근한다.
            그 전에 동글이의 리지드 바디를 public으로 지정한다.
            그리고 시뮬레이트를 false
        [l]. Hide() 함수에서도 만약 게임 매니저에 전달한 매개 변수를 받으면 이펙트를 실행 시킨다.
        [m]. 게임 오버가 된 후에도 코루틴이 계속해서 호출되며 게임이 진행된다.
            동글이 호출함수에서 게임 오버 플래그를 확인한 뒤 반환한다.
*/

/*
6. 물리 퍼즐 게임 - 채널링이 포함된 사운드 시스템

    #1. 배경 음악
        [a]. 게임 매니저에 오디오 소스를 추가한다.
            이름 BGM 플레이어로 BGM을 등록한다.
                플레이 온 어웨이크를 해제하고 루프는 체크한다.
        [b]. 게임 매니저에서 BGM을 플레이 시키기 위해 오디오 소스를 속성으로 받는다.
        [c]. Start() 함수에서 플레이 한다.

    #2. 효과음 관리
        [a]. 게임 매니저에 오디오 소스를 추가한다.
            이름 SFX 플레이어로 플레이 온 어웨이크와 루프 모두 해제 한다.
        [b]. SFX 플레이어를 3개 만든다.
            여러개의 효과음이 동시에 플레이 될 수 있도록 한다.
        [c]. 게임 매니저에게 오디오 소스 배열을 속성으로 둔다.
            현재 실행할 오디오 인덱스도 속성으로 둔다.
        [d]. 효과음을 담을 오디오 클립 배열을 속성으로 둔다.
            열거형으로 효과음을 구분할 상수를 속성으로 둔다.
                레벨업, 다음, 어테치, 버튼, 게임 오버
        [e]. 속성에 SFX 플레이어 전달
            속성에 오디오 클립 전달
        [f]. 효과음 플레이 함수를 만든다.
            매개 변수로 열거형 타입을 받는다.
            스우치 문으로 열거형 타입에 따른 각기 다른 효과음을 출력하도록 한다.
                레벨업일 경우 SFX플레이어 배열에 인덱스를 전달하여 클립을 저장한다.
                다음일 경우 해당 클립을 저장
                어테치도 해당 클립 저장
                버튼도, 게임 오버도 각각 효과음을 배열에 저장한다.
            스위치 문을 나와서 해당 클립을 재생한다.
            인덱스를 ++
                OutOfRange에러를 방지 하기 위해 인덱스 + 1 % sfxPlayer.Length

    #3. 각 상황에 따른 효과음 배치
        [a]. 레벨업
            동글이 스크립트가 레벨업을 한다.
            이펙트를 실행할 때 게임 매니저에 접근하여 sfxPlay함수에 열거형 상수를 전달한다.
        [b]. 다음
            동글이 호출 함수에서 동글이를 활성화 한 뒤 sfxPlay함수에 열거형 상수를 전달한다.
        [c]. 게임 오버
            동글이가 모두 지워진 뒤 1초 정도 쉬고 sfxPlay함수에 열거형 상수를 전달한다.
        [d]. 어테치는 어느 사물이든 부딫칠때 나는 효과음이다.
        [e]. 동글이가 충돌을 할 때 호출되는 이벤트 함수를 만든다.
            코루틴 어테치 루틴 함수를 만든다.
                충돌음 딜레이 시간을 0.2초 정도로 둔다.
                현재 어테치 중인지 확일할 플래그를 속성으로 둔다.
                코루틴이 호출되는 순간 플래그에 true
                0.2초 뒤에 false
                함수 초반에 현재 플래그가 true인지 확인하고 break
                어테치가 true일 때는 sfxPlay함수에 열거형 상수를 전달한다.
            이벤트 함수가 코루틴을 호출한다.
*/

/*
7. 물리 퍼즐 게임 - 쉽게 구현해보는 오브젝트풀링

    #1. 풀 생성
        [a]. 게임 매니저에 동글이 타입의 리스트와, 파티클 시스템 타입의 리스트, 풀 사이즈를 속성으로 갖는다.
            인스펙터 상에서 풀 사이즈 속성을 조작하기 쉽게 하기 위해 키워드를 추가한다.
                [Range(1, 30)]
        [b]. 현재 오브젝트 풀링에 어느 부분을 참고하고 있는지 지시해 주는 정수형 변수를 추가로 속성으로 갖는다.
        [c]. 동글이 생성 함수에서 진행하던 인스턴트에이트를 동글이 만들기 함수로 옮긴다.
        [d]. 이펙트 인스턴스에게 이름을 지정해 준다.
            이펙트 컴포넌트를 가져온 뒤 리스트에 저장한다.
        [e]. 동글이도 마찬가지로 이름을 지정하고 컴포넌트를 가져온 뒤 리스트에 저장한다.
            이 때 동글이는 매니저를 가지고 있으므로 인스턴스를 만들 때 동글이에게 자신을 넘겨 주도록 한다.
        [f]. Awake()에서 동글이 리스트를 초기화 한 뒤에 반복문으로 동글이 만들기 함수를 호출한다.
            기존에 동글이를 만들던 함수는 임시로 null을 반환하도록 한다.

    #2. 풀 사용
        [a]. 기존 동글이 생성 함수는 이미 만들어진 동글이 중 활성화된 동글이를 가져오도록 한다.
            반복문으로 리스트를 탐색한다.
                풀 커서에 1을 더하고 풀의 최대 크기의 나눈 몫을 저장한다.
                풀 커서가 가리키는 풀의 동글이가 활성화 되어 있지 않다면 해당 동글이를 반환한다.
        [b]. 그런데 만약 반복문을 다 돌아도 비활성화된 동글이를 찾지 못하였다면?
            동글이 만드는 함수를 반환하도록 한다.
        [c]. 동글이 호출 함수에서는 동글이 변수를 만들어서 속성에 저장할 필요 없이 바도 기존 동글이 생성 함수를 속성에 배정한다.

    #3. 동글이가 합쳐질 때 비활성화 되는 동글이가 다시 활성화될 때 재사용 로직
        [a]. 동글이가 HideRoutine()에서 자기 자신을 비활성화 하는데 이때 자신에 대한 모든 정보를 초기화 하는 작업을 진행한다.
        [b]. 동글이가 비활성화 될때 호출되는 이벤트 함수를 만든다.
        [c]. 레벨, 드래그, 머지, 어테치, 로컬 위치(현재 동글이 그룹에 있으므로), 로컬 방향, 로컬 크기
        [d]. 리지드바디 시뮬레이트 끄기, 속도 zero, 회전 속도 0, 서클 콜라이더 활성화
*/

/*
8. 물리 퍼즐 게임 - 모바일 게임으로 완성하기

    #1. 변수 정리
        [a]. 게임 매니저의 변수를 카테고리 별로 분류해 둔다.
            변수 위에 [Header("-------[Core]")] : isOver, score, maxLevel
            변수 위에 [Header("-------[Object Pooling]")] : 프리팹들, 그룹, 리스트, 사이즈, 커서, 동글이
            변수 위에 [Header("-------[Audio]")] : 오디오 소스, 클립, 열거형 타입

    #2. 점수 시스템 완성
        [a]. UI를 사용하기 위해 네임 스페이스 UnityEngine.UI 추가
        [b]. UI 헤더를 새로 만들고 점수 텍스트를 속성으로 갖는다.
        [c]. 씬에 있는 캔버스에 Text UI를 추가 
            캔버스에서 모바일 마다 해상도가 다르기 때문에 캔버스 스케일러에서 모드를 스크린 사이드로 바꾼다.
            450 * 950
            텍스트의 앵커를 좌측 상단으로 설정
            크기는 0, 0 : Overflow
            폰트, 사이즈, 정렬
            스코어 텍스트로 명명
        [d]. 게임 매니저에서 현재 점수를 스코어 텍스트를 통해서 출력한다.
        [e]. LateUpdate() 함수를 만든다.
            스코어 텍스트를 통해서 게임 매니저의 점수를 전달한다.
                이때 정수형인 점수를 텍스트로 바꾸는 작업을 진행 한다.
        [f]. 최고점수는 따로 저장하여서 출력하도록 한다.
            캔버스에 최고 점수를 출력할 텍스트 UI를 만든다.
            게임 매니저 속성으로 최고 점수를 담을 텍스트 변수를 만든다.
        [g]. Awake() 함수에서 PlayerPrefs로 데이터를 불러온다.
            불러온 정수 값은 형변환을 하여 텍스트로 출력한다.
            그런데 만약 저장된 점수가 없을 경우 0점을 저장하도록 한다.
            게임 오버 코루틴 함수로 가서 최종 점수와 최고 점수를 비교하여 최종 점수가 더 높다면 저장한다.

    #3. 게임 종료 UI
        [a]. 캔버스에 이미지 만들어서 게임 오버 UI를 모두 담는다.
            화면을 가득 채운다.
            알파값을 낮추어 준다.
        [b]. 게임 오버 그룹에 이미지를 추가하여 종료를 알린다.
            샛 네이티브 사이즈로 크기를 맞춘다.
        [c]. 재시작 버튼도 만든다.
            버튼에 이미지 부착
            버튼에 최종 점수를 출력한다.
        [d]. 코드를 통해서 컨트롤 한다.
            게임이 끝날 때 현재 점수를 최종 점수와 동기화 한다.
            게임이 끝날 때 종료 UI를 활성화 시킨다.
        [e]. 게임 종료 UI 그룹을 게임 오브젝트로, 최종 점수 텍스트를 속성으로 받는다.
        [f]. 최고 점수를 갱신 한 뒤에 종료 UI를 활성화 한다.
            최종 점수에 현재 점수를 배정한다.
        [g]. 재시작 버튼과 연동될 함수를 만든다.
            버튼 오디오를 플레이 하고, 코루틴 재시작 함수를 만든다.
        [h]. 1초 정도 쉬었다가 씬을 새로 불러온다.
            씬 매니지먼트를 네임 스페이스로 등록한다.
        [i]. 재시작 함수에서 코루틴 함수를 호출한다.
            버튼에 함수를 등록한다.
        [j]. 게임 오버가 될 때 BGM을 멈추도록 한다.

    #4. 게임 시작
        [a]. 게임 종료 그룹이 있었듯이 게임 시작 그룹을 만든다.
        [b]. 타이틀 이미지를 부착한다.
        [c]. 점수 텍스트를 삭제하도록 한다.
        [d]. 현재 점수와 최고 점수와 플레이 그라운드를 모두 비활성화 한다.
        [e]. 게임 매니저에서 게임 시작시 활성화 할 게임 오브젝트들과 게임 시작 그룹을 속성으로 받는다.
            게임 오브젝트 (경계선, 바닥)
        [f]. 기존 Start() 함수를 GameStart()로 명명
            경계선, 바닥, 점수들을 활성화 한다.
            게임 시작 그룹을 비활성화 한다.
            버튼을 눌렀으니 버튼 효과음을 플레이 한다.
        [g]. 인보크 함수로 동글이 호출 함수를 호출한다.
        [h]. Update() 함수를 만들고 종료 버튼 다운을 감지한다.
            뒤로가기 버튼이 눌렸다면 Application.Quit() 게임으 종료 한다.
        [i]. 게임 시작 버튼에 함수를 등록한다.

    #5. 모바일 빌드
        [a]. 빌드 세팅에서 플레이어 세팅을 세팅한다.
            회사 이름, 앱의 이름, 아이콘
            리솔루션에서 세로: 초상화 모드, 가로 모드는 해제
            아더세팅에서 컨피규레이션 스크립팅 백그라운드 IL2CPP로, ARM64로 64비트 체크
        [b]. 빌드
*/