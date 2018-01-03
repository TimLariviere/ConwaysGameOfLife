namespace FSharp

module Game =
    type CellState = | ALIVE | DEAD
    
    let mapWidth = 50
    let mapHeight = 50
    let rulesBecomeAlive = [ 0; 3 ]
    let rulesStayAlive = [ 2; 3; 5 ]
    
    let toInt (map: CellState array) index =
        if (index >= 0 && index < map.Length) then
            match map.[index] with
            | ALIVE -> 1
            | DEAD -> 0
        else 0
        
    let private getLivingNeighbors map index =
        let toInt = toInt map
        let topLeft = index - mapWidth - 1 |> toInt
        let top = index - mapWidth |> toInt
        let topRight = index - mapWidth + 1 |> toInt
        let left = index - 1 |> toInt
        let right = index + 1 |> toInt
        let bottomLeft = index + mapWidth - 1 |> toInt
        let bottom = index + mapWidth |> toInt
        let bottomRight = index + mapWidth + 1 |> toInt      
        
        topLeft + top + topRight + left + right + bottomLeft + bottom + bottomRight
        
    let private checkRules rules neighborsCount =
        if (List.contains neighborsCount rules) then ALIVE else DEAD
        
    let private checkBecomeAliveRules = checkRules rulesBecomeAlive
    let private checkStayAliveRules = checkRules rulesStayAlive
        
    let private getNewState map index currentState =
        let neighborsCount = getLivingNeighbors map index
        match currentState with
        | ALIVE -> checkStayAliveRules neighborsCount
        | DEAD -> checkBecomeAliveRules neighborsCount
    
    let createEmptyMap() =
        Array.init (mapWidth * mapHeight) (fun i -> DEAD)
        
    let computeNextStep map =
        let getNewStateForMap = getNewState map            
        map |> Array.mapi getNewStateForMap
        
        
    createEmptyMap () |> computeNextStep |> printfn "%A";;