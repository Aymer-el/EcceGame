using UnityEngine;

public class Multiplayer : Global
{
    public override void UpdateMouseOver()
    {
        if (Camera.main && Input.GetMouseButtonDown(0) && CanPlay())
        {
            bool physicsEcceBoard = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
               out RaycastHit hit, 25.0f, LayerMask.GetMask("EcceBoard"));
            bool physicsWhiteBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
              out RaycastHit hit1, 50f, LayerMask.GetMask("WhiteBanch"));
            bool physicsBlackBanch = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
              out RaycastHit hit2, 50f, LayerMask.GetMask("BlackBanch"));
            if (physicsEcceBoard || physicsWhiteBanch || physicsBlackBanch)
            {
                // Saving mouseOver
                mouseOver.x = (int)hit.point.x;
                mouseOver.y = (int)hit.point.z;
                if (Input.GetMouseButtonDown(0))
                {
                    if (selectedPiece == null)
                    {
                        // Selecting a new piece
                        if (physicsWhiteBanch || physicsBlackBanch)
                        {
                            if (ClientTCP.connected)
                            {
                                string msg = "CPLA|";
                                msg += ClientTCP.roomNumber.ToString() + '|';
                                msg += ClientTCP.players.ToString() + '|';
                                msg += player.ToString();
                                ClientTCP.SendCMove(msg);
                            }
                        }
                        else
                        // Selecting a piece
                        {
                            if (ClientTCP.connected)
                            {
                                string msg = "CSEL|";
                                msg += ClientTCP.roomNumber.ToString() + '|';
                                msg += ClientTCP.players.ToString() + '|';
                                msg += player.ToString() + "|";
                                msg += mouseOver.x + "|";
                                msg += mouseOver.y;
                                ClientTCP.SendCMove(msg);
                            }
                        }
                    }
                    else
                    {
                        if (ClientTCP.connected)
                        {
                            string msg = "CMOV|";
                            msg += ClientTCP.roomNumber.ToString() + '|';
                            msg += ClientTCP.players.ToString() + '|';
                            msg += player.ToString() + "|";
                            msg += mouseOver.x + "|";
                            msg += mouseOver.y + "|";
                            msg += startDrag.x + "|";
                            msg += startDrag.y;
                            ClientTCP.SendCMove(msg);
                        }
                    }
                }
            }

        }
    }
}
