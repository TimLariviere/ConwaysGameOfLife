namespace FSharp

module Game =
    type CellState = | ALIVE | DEAD
    
    let mapWidth = 50
    let mapHeight = 50
    let rulesBecomeAlive = [| 0; 3 |]
    let rulesStayAlive = [| 2; 3; 5 |]

    let private transformPos (x, y) xMod yMod =
        (x + xMod, y + yMod)
    
    let private posToInt (map: CellState[,]) (x, y) =
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight) then
            match map.[x, y] with
            | ALIVE -> 1
            | DEAD -> 0
        else 0

    let private getLivingNeighbors map x y =
        let transformPos = transformPos (x, y)
        let posToInt = posToInt map

        let topLeft     = transformPos -1 -1 |> posToInt
        let top         = transformPos  0 -1 |> posToInt
        let topRight    = transformPos  1 -1 |> posToInt
        let left        = transformPos -1  0 |> posToInt
        let right       = transformPos  1  0 |> posToInt
        let bottomLeft  = transformPos -1  1 |> posToInt
        let bottom      = transformPos  0  1 |> posToInt
        let bottomRight = transformPos  1  1 |> posToInt
        
        topLeft + top + topRight + left + right + bottomLeft + bottom + bottomRight
        
    let private checkRules rules neighborsCount = 
        if (Array.contains neighborsCount rules) then ALIVE else DEAD
        
    let private checkBecomeAliveRules = checkRules rulesBecomeAlive
    let private checkStayAliveRules = checkRules rulesStayAlive
        
    let private getNewState map x y currentState =
        let neighborsCount = getLivingNeighbors map x y
        match currentState with
        | ALIVE -> checkStayAliveRules neighborsCount
        | DEAD -> checkBecomeAliveRules neighborsCount
    
    let createEmptyMap() =
        Array2D.init mapWidth mapHeight (fun x y -> DEAD)
        
    let computeNextStep map =
        let getNewStateForMap = getNewState map            
        map |> Array2D.mapi getNewStateForMap