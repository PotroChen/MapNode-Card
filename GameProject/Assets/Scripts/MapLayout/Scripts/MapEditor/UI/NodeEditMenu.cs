using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class NodeEditMenu : MonoBehaviour
    {
        private RectTransform rectTransform;
        private MapNodeEditView currentNodeView;

        public Button AddButton;
        public Button DeleteButton;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            AddButton.onClick.RemoveListener(AddBtn_OnClick);
            AddButton.onClick.AddListener(AddBtn_OnClick);

            DeleteButton.onClick.RemoveListener(DeleteBtn_OnClick);
            DeleteButton.onClick.AddListener(DeleteBtn_OnClick);
        }

        private void OnDestroy()
        {
            AddButton.onClick.RemoveListener(AddBtn_OnClick);
            DeleteButton.onClick.RemoveListener(DeleteBtn_OnClick);
        }
        public void Show(MapNodeEditView nodeView)
        {
            gameObject.SetActive(true);
            var nodeViewTra = nodeView.Content;

            var menuPosX = nodeViewTra.position.x + nodeViewTra.rect.width * 0.5f * nodeViewTra.lossyScale.x;
            var menuPosY = nodeViewTra.position.y + nodeViewTra.rect.height * 0.5f * nodeViewTra.lossyScale.x;

            var mainCamera = Camera.main;
            var worldPos = new Vector3 (menuPosX,  menuPosY, 0f) ;
            var screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);

            var canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, mainCamera,out var localPoint);

            var offset = Vector2.one*10f;
            rectTransform.localPosition = localPoint + offset;
            currentNodeView = nodeView;
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void AddBtn_OnClick()
        {
            ShowDirectorSelector((MapDirection direction) =>
            {
                var targetPos = currentNodeView.Data.Position;
                switch (direction)
                {
                    case MapDirection.N:
                        targetPos = targetPos + Vector2Int.up;
                        break;
                    case MapDirection.E:
                        targetPos = targetPos + Vector2Int.right;
                        break;
                    case MapDirection.S:
                        targetPos = targetPos + Vector2Int.down;
                        break;
                    case MapDirection.W:
                        targetPos = targetPos + Vector2Int.left;
                        break;
                }
                var newNodeView = currentNodeView.LayoutView.CreateNode("New Node", targetPos);
                currentNodeView.Data.AddConnectedNode(newNodeView.Data);
                Hide();
            });
        }

        private void DeleteBtn_OnClick()
        {
            currentNodeView.LayoutView.DestroyNode(currentNodeView);
            currentNodeView = null;
            Hide();
        }

        #region DirectionSelector
        public GameObject DirectionSelectorGo;
        public Button N_Btn;
        public Button E_Btn;
        public Button S_Btn;
        public Button W_Btn;

        private void ShowDirectorSelector(Action<MapDirection> onDirectionSelected)
        {
            DirectionSelectorGo.SetActive(true);
            N_Btn.onClick.RemoveAllListeners();
            E_Btn.onClick.RemoveAllListeners();
            S_Btn.onClick.RemoveAllListeners();
            W_Btn.onClick.RemoveAllListeners();

            N_Btn.onClick.AddListener(() =>
            {
                onDirectionSelected?.Invoke(MapDirection.N);
                HideDirectorSelector();
            });
            E_Btn.onClick.AddListener(() =>
            {
                onDirectionSelected?.Invoke(MapDirection.E);
                HideDirectorSelector();
            });
            S_Btn.onClick.AddListener(() =>
            {
                onDirectionSelected?.Invoke(MapDirection.S);
                HideDirectorSelector();
            });
            W_Btn.onClick.AddListener(() =>
            {
                onDirectionSelected?.Invoke(MapDirection.W);
                HideDirectorSelector();
            });
        }

        private void HideDirectorSelector()
        {
            DirectionSelectorGo.SetActive(false);
        }


        #endregion
    }

}