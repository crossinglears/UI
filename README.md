# Lears UI

## Overview
Extra UI Tools and helpers for Unity Engine.
-Contains ready-to-use gameobjects

## Features/Contents
- `UI_Moveable` – Drag and move UI elements
    How to use:
        - Add to a UI object
        - Change the ContentRectTransform if it's gonna move other object rather than the UI object that you attached it to
        - Adjust paddings as needed

- `UI_Collapsible` – Slides a panel to show/hide as needed
    How to use:
        - Attach to an object
        - Select a TargetRectTransform, this will be the object that will collapse
        - Decide a CollapsePosition, this will tell how you want the UI_Collapsible to hide
        - Select an anchor preset for the TargetRectTransform ()
            = Hold SHIFT + ALT then select a non-stretching preset depending on your chosen CollapsePosition

- `UI_Resizable` – Resizes an object by pulling a selected image

### Edited UI Components:
- `CL_Toggle` - exposed an OnTrue and OnFalse event. And made the toggle activate at Start()
- `CL_ToggleGroup` - Needed by the custom CL_Toggle

For any question or suggestion please leave us a message at crossinglears@gmail.com
